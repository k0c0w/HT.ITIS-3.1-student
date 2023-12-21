using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;

namespace Dotnet.Homeworks.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommand : ICommand, IAmOrderOwner
{
    public UpdateOrderCommand(Guid orderId, IEnumerable<Guid> productsIds)
    {
        OrderId = orderId;
        ProductsIds = productsIds;
    }

    public Guid OrderId { get; init; }
    public IEnumerable<Guid> ProductsIds { get; init; }
}