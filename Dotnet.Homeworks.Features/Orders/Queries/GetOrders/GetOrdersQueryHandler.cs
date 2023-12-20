using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrder;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Shared.Dto;
using Dotnet.Homeworks.Shared.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Dotnet.Homeworks.Features.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, GetOrdersDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<GetOrdersQueryHandler> _logger;

    private HttpContext HttpContext { get; }

    public GetOrdersQueryHandler(ILogger<GetOrdersQueryHandler> logger, IOrderRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        HttpContext = httpContextAccessor.HttpContext!;
        _orderRepository = repository;
        _logger = logger;
    }


    public async Task<Result<GetOrdersDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var aplicantId = HttpContext.User.GetUserId();
        if (aplicantId == null)
            return new Result<GetOrdersDto>(default, false, "User was not found.");

        try
        {
            var orders = await _orderRepository.GetAllOrdersFromUserAsync(aplicantId.Value, cancellationToken);

            return new Result<GetOrdersDto>(new GetOrdersDto(orders.Select(x => new GetOrderDto(x.Id, x.ProductsIds))), true);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message);

            return new Result<GetOrdersDto>(default, false, error: "An error occured while retrieving orders.");
        }
    }
}