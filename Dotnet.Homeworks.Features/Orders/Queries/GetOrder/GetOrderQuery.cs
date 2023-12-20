using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;

namespace Dotnet.Homeworks.Features.Orders.Queries.GetOrder;

public class GetOrderQuery : IQuery<GetOrderDto>
{
    public GetOrderQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; init; }
}