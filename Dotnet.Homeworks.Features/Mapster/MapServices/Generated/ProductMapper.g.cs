using System.Collections.Generic;
using System.Linq;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Mapster.MapServices.Abstractions;
using Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;

namespace Dotnet.Homeworks.Features.Mapster.MapServices.Abstractions
{
    public partial class ProductMapper : IProductMapper
    {
        public GetProductsDto MapFromProducts(IEnumerable<Product> p1)
        {
            return p1 == null ? null : new GetProductsDto(p1 == null ? null : p1.Select<Product, GetProductDto>(funcMain1));
        }
        public Product MapFromUpdateCommandToExisting(UpdateProductCommand p3, Product p4)
        {
            if (p3 == null)
            {
                return null;
            }
            Product result = p4 ?? new Product();
            
            result.Name = p3.Name;
            return result;
            
        }
        
        private GetProductDto funcMain1(Product p2)
        {
            return p2 == null ? null : new GetProductDto(p2.Id, p2.Name);
        }
    }
}