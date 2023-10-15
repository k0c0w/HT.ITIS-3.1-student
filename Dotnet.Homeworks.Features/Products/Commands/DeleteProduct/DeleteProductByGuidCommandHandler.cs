using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;

namespace Dotnet.Homeworks.Features.Products.Commands.DeleteProduct;

internal sealed class DeleteProductByGuidCommandHandler : ICommandHandler<DeleteProductByGuidCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public DeleteProductByGuidCommandHandler(IUnitOfWork unitOfWork)
    {
        _productRepository = unitOfWork.ProductRepository;
        _unitOfWork = unitOfWork;
    }

    public Task Handle(DeleteProductByGuidCommand request, CancellationToken cancellationToken)
    {
        return _productRepository.DeleteProductByGuidAsync(request.Guid, cancellationToken)
            .ContinueWith((completedTask) =>
            {
                if (!(completedTask.IsFaulted || cancellationToken.IsCancellationRequested))
                {
                    return _unitOfWork.SaveChangesAsync(cancellationToken);
                }

                return Task.CompletedTask;
            });
    }
}