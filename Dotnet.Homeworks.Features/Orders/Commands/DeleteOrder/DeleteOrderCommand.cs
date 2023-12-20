using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;

namespace Dotnet.Homeworks.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderByGuidCommand : ICommand
{
    public DeleteOrderByGuidCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; init; }
}