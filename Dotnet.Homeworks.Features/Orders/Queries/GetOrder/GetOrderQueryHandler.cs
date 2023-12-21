using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Mapster.MapServices.Abstractions;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.Extensions.Logging;

namespace Dotnet.Homeworks.Features.Orders.Queries.GetOrder;

public class GetOrderQueryHandler : IQueryHandler<GetOrderQuery, GetOrderDto>
{
    private readonly ILogger<GetOrderQueryHandler> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderMapper _orderMapper;

    public GetOrderQueryHandler(ILogger<GetOrderQueryHandler> logger, IOrderRepository orderRepository, IOrderMapper orderMapper)
    {
        _logger = logger;
        _orderMapper = orderMapper;
        _orderRepository = orderRepository;
    }

    public async Task<Result<GetOrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetOrderByGuidAsync(request.Id, cancellationToken);
            if (order == null)
                return new Result<GetOrderDto>(default, false, error: "Order was not found");

            return new Result<GetOrderDto>(_orderMapper.MapFromOrder(order), true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new Result<GetOrderDto>(default, false, error: $"An error occured while retriving order ({request.Id})");
        }
    }
}