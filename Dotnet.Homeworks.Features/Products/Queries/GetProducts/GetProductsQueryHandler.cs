using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Mapster.MapServices.Abstractions;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Queries.GetProducts;

internal sealed class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, GetProductsDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductMapper _productMapper;


    public GetProductsQueryHandler(IProductRepository productRepository, IProductMapper productMapper)
    {
        _productRepository = productRepository;
        _productMapper = productMapper;
    }

    async Task<Result<GetProductsDto>> IRequestHandler<GetProductsQuery, Result<GetProductsDto>>.Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var products = await _productRepository.GetAllProductsAsync(cancellationToken);

        return new Result<GetProductsDto>(_productMapper.MapFromProducts(products), true);
    }
}