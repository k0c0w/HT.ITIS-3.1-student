using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;
using MediatR;

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


    async Task<Result<Guid>> IRequestHandler<InsertProductCommand, Result<Guid>>.Handle(InsertProductCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var newProduct = new Product
        {
            Name = request.Name,
        };

        try
        {
            var productId = await _productRepository.InsertProductAsync(newProduct, cancellationToken).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return new Result<Guid>(productId, true);
        }
        catch (Exception ex)
        {
            return new Result<Guid>(default, false, ex.Message);
        }
        finally 
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}