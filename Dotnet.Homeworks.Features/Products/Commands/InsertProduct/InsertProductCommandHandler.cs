using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;

namespace Dotnet.Homeworks.Features.Products.Commands.InsertProduct;

internal sealed class InsertProductCommandHandler : ICommandHandler<InsertProductCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;


    public InsertProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _productRepository = unitOfWork.ProductRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(InsertProductCommand request, CancellationToken cancellationToken)
    {
        var newProduct = new Product
        {
            Name = request.Name,
        };

        var productId = await _productRepository.InsertProductAsync(newProduct, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return productId;
    }
}