using Dotnet.Homeworks.Features.Products.Commands.DeleteProduct;
using Dotnet.Homeworks.Features.Products.Commands.InsertProduct;
using Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Homeworks.MainProject.Controllers;

[ApiController]
public class ProductManagementController : ControllerBase
{
    private static readonly GetProductsQuery GetProductsQuery = new GetProductsQuery();

    private readonly ISender _cqrsSender;

    public ProductManagementController(ISender cqrs)
    {
        _cqrsSender = cqrs;
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProductsAsync(CancellationToken cancellationToken)
    {
        var productsQueryResult = await _cqrsSender.Send(GetProductsQuery, cancellationToken).ConfigureAwait(false);

        return productsQueryResult
            ? new JsonResult(productsQueryResult.Value)
            : StatusCode(500);
    }

    [HttpPost("product")]
    public async Task<IActionResult> InsertProduct([FromBody] string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(name))
            return BadRequest(new { error = "Name can not be empty" });

        var insertProductCommandResult = await _cqrsSender.Send(new InsertProductCommand(name), cancellationToken).ConfigureAwait(false);

        return insertProductCommandResult
            ? Created(Url.Action(nameof(GetProductsAsync), nameof(ProductManagementController)), insertProductCommandResult.Value)
            : StatusCode(500);
    }

    [HttpDelete("product")]
    public async Task<IActionResult> DeleteProduct(Guid guid, CancellationToken cancellationToken)
    {
        if (guid == default)
            return BadRequest(new { error = "Wrong guid" });

        var commandResult = await _cqrsSender.Send(new DeleteProductByGuidCommand(guid), cancellationToken).ConfigureAwait(false);

        return commandResult ? NoContent() : NotFound(guid);
    }

    [HttpPut("product")]
    public async Task<IActionResult> UpdateProduct(Guid guid, string name, CancellationToken cancellationToken)
    {
        if (guid == default)
            return BadRequest(new { error = "Wrong guid" });

        var commandResult = await _cqrsSender.Send(new UpdateProductCommand(guid, name), cancellationToken).ConfigureAwait(false);

        return commandResult ? NoContent() : NotFound(guid);
    }
}