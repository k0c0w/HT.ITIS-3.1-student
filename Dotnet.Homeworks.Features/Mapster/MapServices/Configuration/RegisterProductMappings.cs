using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;
using Mapster;

namespace Dotnet.Homeworks.Features.Mapster.MapServices.Configuration;

public class RegisterProductMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        {
            config.NewConfig<Product, GetProductDto>()
                .Map(d => d.Guid, s => s.Id)
                .Map(d => d.Name, s => s.Name);
        }

        {
            config.NewConfig<IEnumerable<Product>, GetProductsDto>()
            .Map(d => d.Products, s => s);
        }

        {
            config.NewConfig<UpdateProductCommand, Product>()
                .Ignore(d => d.Id)
                .Map(d => d.Name, s => s.Name)
                .GenerateMapper(MapType.MapToTarget);
        }
    }
}