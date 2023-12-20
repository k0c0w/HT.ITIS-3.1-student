using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.Extensions.Logging;

namespace Dotnet.Homeworks.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : ICommandHandler<UpdateOrderCommand>
{
    private readonly ILogger<UpdateOrderCommandHandler> _logger;
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderCommandHandler(ILogger<UpdateOrderCommandHandler> logger, IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }

    public async Task<Result> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetOrderByGuidAsync(request.OrderId, cancellationToken);

            if (order == null)
                return new Result(false, error: $"Order ({request.OrderId}) was not found.");

            if (order.ProductsIds.SequenceEqual(request.ProductsIds))
                return new Result(false, error: $"No order updates were found for order ({request.OrderId}).");

            var updatedOrder = new Order()
            {
                Id = order.Id,
                OrdererId = order.OrdererId,

                ProductsIds = request.ProductsIds,
            };

            await _orderRepository.UpdateOrderAsync(updatedOrder, cancellationToken);

            return new Result(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new Result(false, error: $"An error occured while updating order`s ({request.OrderId})  products list.");
        }
    }
}