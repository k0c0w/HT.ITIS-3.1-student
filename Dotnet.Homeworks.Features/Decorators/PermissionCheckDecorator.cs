using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Decorators;

public class PermissionCheckDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IPermissionCheck<TRequest>? _permissionCheck;

    private Result<TResponse> Success { get; } = new Result<TResponse>(default, true);

    public PermissionCheckDecorator(IPermissionCheck<TRequest>? permissionCheck)
    {
        _permissionCheck = permissionCheck;
    }

    public async virtual Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        if (request is not IClientRequest userRequest || _permissionCheck is null)
            return Success as dynamic;

        var permissionCheckResult = await _permissionCheck.CheckPermissionAsync(request);
        var failures = permissionCheckResult.Where(x => x.IsFailure).ToArray();
        if (failures.Length == 0)
            return Success as dynamic;

        return new Result<TResponse>(default, false, string.Join(' ', failures.Select(x => x.Error))) as dynamic;
    }
}
