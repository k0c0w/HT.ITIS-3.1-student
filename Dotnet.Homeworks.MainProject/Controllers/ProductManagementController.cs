using Dotnet.Homeworks.Features.Products.Commands.DeleteProduct;
using Dotnet.Homeworks.Features.Products.Commands.InsertProduct;
using Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;
using Dotnet.Homeworks.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Homeworks.MainProject.Controllers;

[ApiController]
public class ProductManagementController : ControllerBase
{
    private static readonly GetProductsQuery GetProductsQuery = new GetProductsQuery();

    private readonly IMediator _mediator;

    public ProductManagementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProductsAsync(CancellationToken cancellationToken)
    {
        var productsQueryResult = await _mediator.Send(GetProductsQuery, cancellationToken);

        return productsQueryResult.IsSuccess
            ? new JsonResult(productsQueryResult.Value)
            : BadRequest();
    }

    [HttpPost("product")]
    public async Task<IActionResult> InsertProduct([FromBody] string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(name))
            return BadRequest(new { error = "Name can not be empty" });

        var insertProductCommandResult = await _mediator.Send(new InsertProductCommand(name), cancellationToken);

        return insertProductCommandResult.IsSuccess
            ? Created(Url?.Action(nameof(GetProductsAsync), nameof(ProductManagementController)) ?? string.Empty, insertProductCommandResult.Value)
            : BadRequest("Model can not be inserted");
    }

    [HttpDelete("product")]
    public async Task<IActionResult> DeleteProduct(Guid guid, CancellationToken cancellationToken)
    {
        if (guid == default)
            return BadRequest(new { error = "Wrong guid" });

        var commandResult = await _mediator.Send(new DeleteProductByGuidCommand(guid), cancellationToken);

        return commandResult.IsSuccess ? NoContent() : NotFound(guid);
    }

    [HttpPut("product")]
    public async Task<IActionResult> UpdateProduct(Guid guid, string name, CancellationToken cancellationToken)
    {
        if (guid == default)
            return BadRequest(new { error = "Wrong guid" });

        var commandResult = await _mediator.Send(new UpdateProductCommand(guid, name), cancellationToken);

        return commandResult.IsSuccess ? NoContent() : NotFound(guid);
    }
}