using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Products.Commands.InsertProduct;
using Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;
using Mapster;

namespace Dotnet.Homeworks.Features.Mapster.MapServices.Abstractions;

[Mapper]
public interface IProductMapper
{
    GetProductsDto MapFromProducts(IEnumerable<Product> product);

    Product MapFromUpdateCommandToExisting(UpdateProductCommand command, Product existingProduct);
}