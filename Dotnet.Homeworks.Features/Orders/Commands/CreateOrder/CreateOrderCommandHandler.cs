using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Http;
using Dotnet.Homeworks.Shared.Extensions;
using Dotnet.Homeworks.Domain.Entities;
using Microsoft.Extensions.Logging;
using Dotnet.Homeworks.DataAccess.Repositories;

namespace Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    private HttpContext HttpContext { get; }

    public CreateOrderCommandHandler(IOrderRepository orderRepository,
        IProductRepository products,
        IHttpContextAccessor httpContextAccessor,
        ILogger<CreateOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _productRepository = products;
        _logger = logger;
        HttpContext = httpContextAccessor.HttpContext!;
    }

    public async Task<Result<CreateOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var ordererId = HttpContext.User.GetUserId();
        if (ordererId == null)
            return new Result<CreateOrderDto>(default, false, error: "User was not found.");

        try
        {
            var productsExist = request.ProductsIds.Any() && (await CheckOrderProductsExistanceAsync(request.ProductsIds, cancellationToken));
            if (!productsExist)
                return new Result<CreateOrderDto>(default, false, error: "Provide existing products with order.");

            var order = new Order()
            {
                ProductsIds = request.ProductsIds,
                OrdererId = ordererId.Value,
            };

            var orderId = await _orderRepository.InsertOrderAsync(order, cancellationToken);

            return new Result<CreateOrderDto>(new CreateOrderDto(orderId), true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return new Result<CreateOrderDto>(default, false, error: "An error occured while creating order.");
        }
    }

    private async Task<bool> CheckOrderProductsExistanceAsync(IEnumerable<Guid> productsIds, CancellationToken cancellationToken)
    {
        var productExistsTasks = productsIds
            .Select(x => _productRepository.ExistsByIdAsync(x, cancellationToken));

        await Task.WhenAll(productExistsTasks);

        return productExistsTasks.All(x => x.IsCompletedSuccessfully && x.Result);
    }
}