using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Orders.Commands.UpdateOrder;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrder;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrders;
using Mapster;

namespace Dotnet.Homeworks.Features.Mapster.MapServices.Configuration;

public class RegisterOrderMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // GetOrderDto
        {
            config.NewConfig<Order, GetOrderDto>()
                .Map(d => d.Id, s => s.Id)
                .Map(d => d.ProductsIds, s => s.ProductsIds);
        }

        // GetOrdersDto
        {
            config.NewConfig<IEnumerable<Order>, GetOrdersDto>()
                .Map(d => d.Orders, s => s);
        }
    }
}