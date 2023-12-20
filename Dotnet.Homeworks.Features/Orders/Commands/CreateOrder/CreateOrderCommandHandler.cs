using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Http;
using Dotnet.Homeworks.Shared.Extensions;
using Dotnet.Homeworks.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    private HttpContext HttpContext { get; }

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor, ILogger<CreateOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
        HttpContext = httpContextAccessor.HttpContext!;
    }

    public async Task<Result<CreateOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var ordererId = HttpContext.User.GetUserId();
        if (ordererId == null)
            return new Result<CreateOrderDto>(default, false, error: "User was not found.");

        var order = new Order()
        {
            ProductsIds = request.ProductsIds,
            OrdererId = ordererId.Value,
        };

        try
        { 
            var orderId = await _orderRepository.InsertOrderAsync(order, cancellationToken);

            return new Result<CreateOrderDto>(new CreateOrderDto(orderId), true);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message);

            return new Result<CreateOrderDto>(default, false, error: "An error occured while creating order.");
        }
    }
}