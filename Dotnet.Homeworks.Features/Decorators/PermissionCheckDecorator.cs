using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Mediator;

namespace Dotnet.Homeworks.Features.Decorators;

public class PermissionCheckDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IPermissionCheck<IClientRequest>? _permissionCheck;

    public PermissionCheckDecorator(IPermissionCheck<IClientRequest>? permissionCheck)
    {
        _permissionCheck = permissionCheck;
    }

    public async virtual Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        if (request is not IClientRequest userRequest || _permissionCheck is null)
        {
            return true as dynamic;
        }

        var permissionCheckResult = await _permissionCheck.CheckPermissionAsync(userRequest);
        var failures = permissionCheckResult.Where(x => x.IsFailure).ToArray();
        if (failures.Length == 0)
        {
            return true as dynamic;
        }

        return string.Join(' ', failures.Select(x => x.Error)) as dynamic;
    }
}
