using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Decorators;

public class UserPermissionCheckValidationDecorator<TRequest, TResponse> : ValidationDecorator<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IPermissionCheck<IClientRequest> _permissionCheck;

    public UserPermissionCheckValidationDecorator(IEnumerable<IValidator<TRequest>> validators, IPermissionCheck<IClientRequest> permissionCheck) : base(validators)
    {
        _permissionCheck = permissionCheck;
    }

    public override async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        if (request is not IClientRequest userRequest)
            return await base.Handle(request, cancellationToken);

        var permissionCheckResult = await _permissionCheck.CheckPermissionAsync(userRequest);
        var failures = permissionCheckResult.Where(x => x.IsFailure).ToArray();
        if (failures.Length == 0)
            return await base.Handle(request, cancellationToken);

        if (typeof(TResponse) == typeof(Result))
            return new Result(false, string.Join(' ', failures.Select(x => x.Error))) as dynamic;
        else
            return new Result<TResponse>(default, false, string.Join(' ', failures.Select(x => x.Error))) as dynamic;

    }
}
