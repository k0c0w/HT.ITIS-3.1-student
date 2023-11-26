using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.PipelineBehaviors;

public class AdminPermissionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, Result<TResponse>> where TRequest : IRequest<Result<TResponse>>, IAdminRequest
{
    private readonly IPermissionCheck<TRequest> _permissionCheck;

    public AdminPermissionBehavior(IPermissionCheck<TRequest> permissionCheck)
    {
        _permissionCheck = permissionCheck;
    }

    public async Task<Result<TResponse>> Handle(TRequest request, RequestHandlerDelegate<Result<TResponse>> next, CancellationToken cancellationToken)
    {
        var permissionResult = await _permissionCheck.CheckPermissionAsync(request);

        if (permissionResult.Any(x => x.IsFailure))
        {
            return new Result<TResponse>(default, false, string.Join(' ', permissionResult.Where(x => x.IsFailure && !string.IsNullOrEmpty(x.Error)).Select(x => x.Error)));
        }

        return await next();
    }
}
