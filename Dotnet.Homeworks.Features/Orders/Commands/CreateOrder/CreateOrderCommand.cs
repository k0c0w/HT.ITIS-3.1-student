using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
namespace Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommand : ICommand<CreateOrderDto>, IAmOrderOwner
{
    public CreateOrderCommand(IEnumerable<Guid> productsIds)
    {
        ProductsIds = productsIds;
    }

    public IEnumerable<Guid> ProductsIds { get; init; }

    public Guid OrderId => Guid.Empty;
}