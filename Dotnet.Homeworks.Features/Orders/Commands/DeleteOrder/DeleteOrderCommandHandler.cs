using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.Extensions.Logging;

namespace Dotnet.Homeworks.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderByGuidCommand>
{
    private readonly ILogger<DeleteOrderCommandHandler> _logger;
    private readonly IOrderRepository _orderRepository;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommandHandler> logger)
    {
        _logger = logger;
        _orderRepository = orderRepository; 
    }

    public async Task<Result> Handle(DeleteOrderByGuidCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var orderToDelete = await _orderRepository.GetOrderByGuidAsync(request.OrderId, cancellationToken);

            if (orderToDelete == null)
                return new Result(false, error: $"Order ({request.OrderId}) was not found.");

            await _orderRepository.DeleteOrderByGuidAsync(request.Id, cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new Result(false, error: $"An error occured while deleting order with id: {request.Id}");
        }
    }
}