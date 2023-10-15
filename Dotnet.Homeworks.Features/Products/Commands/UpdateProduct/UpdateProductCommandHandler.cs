using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;

namespace Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _productRepository = unitOfWork.ProductRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var updatedEntity = new Product()
        {
            Id = request.Guid,
            Name = request.Name
        };

        await _productRepository.UpdateProductAsync(updatedEntity, cancellationToken)
            .ContinueWith((completedTask) =>
            {
               if (!(completedTask.IsFaulted || cancellationToken.IsCancellationRequested))
               {
                    return _unitOfWork.SaveChangesAsync(cancellationToken);
               }

               return Task.CompletedTask;
            })
            .ConfigureAwait(false);
    }
}