using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;

namespace Dotnet.Homeworks.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderByGuidCommand : ICommand, IAmOrderOwner
{
    public DeleteOrderByGuidCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; init; }

    public Guid OrderId => Id;
}