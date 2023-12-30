using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrder;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrders;
using Mapster;

namespace Dotnet.Homeworks.Features.Mapster.MapServices.Abstractions;

[Mapper]
public interface IOrderMapper
{
    GetOrderDto MapFromOrder(Order order);

    GetOrdersDto MapFromOrders(IEnumerable<Order> orders);
}