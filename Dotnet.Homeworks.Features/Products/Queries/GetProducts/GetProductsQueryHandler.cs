using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Queries.GetProducts;

internal sealed class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, GetProductsDto>
{
    private readonly IProductRepository _productRepository;


    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    async Task<Result<GetProductsDto>> IRequestHandler<GetProductsQuery, Result<GetProductsDto>>.Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var products = await _productRepository.GetAllProductsAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return new Result<GetProductsDto>(
            new GetProductsDto(products.Select(x => new GetProductDto(x.Id, x.Name))),
            true);
    }
}