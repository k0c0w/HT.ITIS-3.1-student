using Dotnet.Homeworks.Domain.Exceptions;
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
        var products = await _cqrsSender.Send(GetProductsQuery, cancellationToken).ConfigureAwait(false);

        return new JsonResult(products);
    }

    [HttpPost("product")]
    public async Task<IActionResult> InsertProduct([FromBody] string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(name))
            return BadRequest(new { error = "Name can not be empty" });

        var insertedProductId = await _cqrsSender.Send(new InsertProductCommand(name), cancellationToken).ConfigureAwait(false);

        return Created(Url.RouteUrl(GetProductsAsync), insertedProductId);
    }

    [HttpDelete("product")]
    public async Task<IActionResult> DeleteProduct(Guid guid, CancellationToken cancellationToken)
    {
        if (guid == default)
            return BadRequest(new { error = "Wrong guid" });

        try
        {
            await _cqrsSender.Send(new DeleteProductByGuidCommand(guid), cancellationToken).ConfigureAwait(false);
        }
        catch (EntityNotFoundException)
        {
            return NotFound(guid);
        }

        return NoContent();
    }

    [HttpPut("product")]
    public async Task<IActionResult> UpdateProduct(Guid guid, string name, CancellationToken cancellationToken)
    {
        if (guid == default)
            return BadRequest(new { error = "Wrong guid" });

        try
        {
            await _cqrsSender.Send(new UpdateProductCommand(guid, name), cancellationToken).ConfigureAwait(false);
        }
        catch (EntityNotFoundException)
        {
            return NotFound(guid);
        }

        return NoContent();
    }
}