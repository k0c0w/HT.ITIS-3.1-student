using Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;
using Dotnet.Homeworks.Features.Orders.Commands.DeleteOrder;
using Dotnet.Homeworks.Features.Orders.Commands.UpdateOrder;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrder;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrders;
using Dotnet.Homeworks.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Homeworks.MainProject.Controllers;

[ApiController]
public class OrderManagementController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderManagementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/orders")]
    public async Task<IActionResult> GetUserOrdersAsync(CancellationToken cancellationToken)
    {
        var getOrdersQueryResult = await _mediator.Send(new GetOrdersQuery(), cancellationToken);
        return getOrdersQueryResult.IsSuccess ? Ok(getOrdersQueryResult.Value) : BadRequest(getOrdersQueryResult.Error);
    }

    [HttpGet("/orders/{id:guid}")]
    public async Task<IActionResult> GetUserOrdersAsync(Guid id, CancellationToken cancellationToken)
    {
        var getOrderQueryResult = await _mediator.Send(new GetOrderQuery(id), cancellationToken);
        return getOrderQueryResult.IsSuccess ? Ok(getOrderQueryResult.Value) : BadRequest(getOrderQueryResult.Error);
    }

    [HttpPost("/orders")]
    public async Task<IActionResult> CreateOrderAsync([FromBody] IEnumerable<Guid> productsIds,
        CancellationToken cancellationToken)
    {
        var createOrderCommandResult = await _mediator.Send(new CreateOrderCommand(productsIds), cancellationToken);
        return createOrderCommandResult.IsSuccess ? Created("/orders", createOrderCommandResult.Value) : BadRequest(createOrderCommandResult.Error);
    }

    [HttpPut("/orders/{id:guid}")]
    public async Task<IActionResult> UpdateOrderAsync(Guid id, [FromBody] IEnumerable<Guid> productsIds,
        CancellationToken cancellationToken)
    {
        var updateOrderCommandResult = await _mediator.Send(new UpdateOrderCommand(id, productsIds), cancellationToken);
        return updateOrderCommandResult.IsSuccess ? NoContent() : BadRequest(updateOrderCommandResult.Error);
    }

    [HttpDelete("/orders/{id:guid}")]
    public async Task<IActionResult> DeleteOrderAsync(Guid id, CancellationToken cancellationToken)
    {
        var deleteOrderCommandResult = await _mediator.Send(new DeleteOrderByGuidCommand(id), cancellationToken);
        return deleteOrderCommandResult.IsSuccess ? NoContent() : BadRequest(deleteOrderCommandResult.Error);
    }
}