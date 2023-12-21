using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Orders;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Extensions;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.PipelineBehaviors;

internal class OrderOwnerValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>, IAmOrderOwner
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private Guid? ApplicatnId { get; }

    public OrderOwnerValidationBehavior(IOrderRepository orderRepository, IUserRepository userRepository, IHttpContextAccessor httpContext)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        ApplicatnId = httpContext.HttpContext?.User.GetUserId();
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (ApplicatnId == null || ApplicatnId == Guid.Empty 
            || (await _userRepository.GetUserByGuidAsync(ApplicatnId.Value, cancellationToken)) is null)
            return "User not found." as dynamic;

        var order = await _orderRepository.GetOrderByGuidAsync(request.OrderId, cancellationToken);
        if (order != null && order.OrdererId != ApplicatnId)
            return "Access denied." as dynamic;

        return await next();
    }
}
