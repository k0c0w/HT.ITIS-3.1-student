using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;

namespace Dotnet.Homeworks.Features.Products.Commands.UpdateProduct
{
    public static partial class UpdateProductCommandMapper
    {
        public static Product AdaptTo(this UpdateProductCommand p1, Product p2)
        {
            if (p1 == null)
            {
                return null;
            }
            Product result = p2 ?? new Product();
            
            result.Name = p1.Name;
            return result;
            
        }
    }
}