using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Infrastructure.Validation.Decorators;

public class PermissionCheckDecorator<TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>> where TRequest : IRequest<Result<TResponse>>
{
    private readonly IPermissionCheck<TRequest>? _permissionCheck;

    private Result<TResponse> Success { get; } = new Result<TResponse>(default, true);

    public PermissionCheckDecorator(IPermissionCheck<TRequest>? permissionCheck)
    {
        _permissionCheck = permissionCheck;
    }

    public async virtual Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        if (request is not IClientRequest userRequest || _permissionCheck is null)
            return Success;

        var permissionCheckResult = await _permissionCheck.CheckPermissionAsync(request);
        var failures = permissionCheckResult.Where(x => x.IsFailure).ToArray();
        if (failures.Length == 0)
            return Success;

        return new Result<TResponse>(default, false, string.Join(' ', failures.Select(x => x.Error)));
    }
}

public class PermissionCheckDecorator<TRequest> : IRequestHandler<TRequest, Result> where TRequest : IRequest<Result>
{
    private readonly IPermissionCheck<TRequest>? _permissionCheck;

    private Result Success { get; } = new Result(true);

    public PermissionCheckDecorator(IPermissionCheck<TRequest>? permissionCheck)
    {
        _permissionCheck = permissionCheck;
    }

    public async virtual Task<Result> Handle(TRequest request, CancellationToken cancellationToken)
    {
        if (request is not IClientRequest userRequest || _permissionCheck is null)
            return Success;

        var permissionCheckResult = await _permissionCheck.CheckPermissionAsync(request);
        var failures = permissionCheckResult.Where(x => x.IsFailure).ToArray();
        if (failures.Length == 0)
            return Success;

        return new Result(false, string.Join(' ', failures.Select(x => x.Error)));
    }
}
