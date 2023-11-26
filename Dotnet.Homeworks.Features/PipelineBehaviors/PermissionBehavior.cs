using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.PipelineBehaviors;

public class AdminPermissionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, IAdminRequest
{
    private readonly IPermissionCheck<IAdminRequest> _permissionCheck;

    public AdminPermissionBehavior(IPermissionCheck<IAdminRequest> permissionCheck)
    {
        _permissionCheck = permissionCheck;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not IAdminRequest adminRequest)
            return await next();

        var permissionResult = await _permissionCheck.CheckPermissionAsync(adminRequest);

        if (permissionResult.Any(x => x.IsFailure))
        {
            if (typeof(TResponse) == typeof(Result))
                return new Result(false, string.Join(' ', permissionResult.Where(x => x.IsFailure && !string.IsNullOrEmpty(x.Error)).Select(x => x.Error))) as dynamic;
            else
                return new Result<TResponse>(default, false, string.Join(' ', permissionResult.Where(x => x.IsFailure && !string.IsNullOrEmpty(x.Error)).Select(x => x.Error))) as dynamic;
        }

        return await next();
    }
}
