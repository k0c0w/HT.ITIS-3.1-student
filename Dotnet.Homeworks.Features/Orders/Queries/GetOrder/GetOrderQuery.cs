using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;

namespace Dotnet.Homeworks.Features.Orders.Queries.GetOrder;

public class GetOrderQuery : IQuery<GetOrderDto>, IAmOrderOwner
{
    public GetOrderQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; init; }

    public Guid OrderId => Id;
}