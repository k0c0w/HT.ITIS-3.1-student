using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;
using MediatR;

namespace Dotnet.Homeworks.Features.Products.Commands.InsertProduct;

internal sealed class InsertProductCommandHandler : ICommandHandler<InsertProductCommand, InsertProductDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;


    public InsertProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _productRepository = unitOfWork.ProductRepository;
        _unitOfWork = unitOfWork;
    }


    async Task<Result<InsertProductDto>> IRequestHandler<InsertProductCommand, Result<InsertProductDto>>.Handle(InsertProductCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var newProduct = new Product
        {
            Name = request.Name,
        };

        try
        {
            var productId = await _productRepository.InsertProductAsync(newProduct, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new Result<InsertProductDto>(new InsertProductDto(productId), true);
        }
        catch (Exception ex)
        {
            return new Result<InsertProductDto>(default, false, ex.Message);
        }
        finally 
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}