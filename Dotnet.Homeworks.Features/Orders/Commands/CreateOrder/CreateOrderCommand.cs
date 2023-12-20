using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;

namespace Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommand : ICommand<CreateOrderDto>
{
    public CreateOrderCommand(IEnumerable<Guid> productsIds)
    {
        ProductsIds = productsIds;
    }

    public IEnumerable<Guid> ProductsIds { get; init; }
}