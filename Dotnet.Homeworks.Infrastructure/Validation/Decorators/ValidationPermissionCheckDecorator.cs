using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Infrastructure.Validation.Decorators;

public class ValidationPermissionCheckDecorator<TRequest, TResponse> : PermissionCheckDecorator<TRequest, TResponse> where TRequest : IRequest<Result<TResponse>>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPermissionCheckDecorator(IEnumerable<IValidator<TRequest>> validators, IPermissionCheck<TRequest>? permissionCheck) : base(permissionCheck)
    {
        _validators = validators;
    }

    public override async Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var permissionCheckResult = await base.Handle(request, cancellationToken);

        if (permissionCheckResult.IsFailure || !_validators.Any())
            return permissionCheckResult;

        var validationResult = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(request, cancellationToken)));

        var validationErrors = validationResult.Where(x => !x.IsValid).ToArray();

        if (validationErrors.Length == 0)
            return permissionCheckResult;

        return new Result<TResponse>(default, false, string.Join(' ', validationErrors.Select(x => x.Errors.SelectMany(x => x.ErrorMessage))));
    }
}

public class ValidationPermissionCheckDecorator<TRequest> : PermissionCheckDecorator<TRequest> where TRequest : IRequest<Result>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPermissionCheckDecorator(IEnumerable<IValidator<TRequest>> validators, IPermissionCheck<TRequest>? permissionCheck) : base(permissionCheck)
    {
        _validators = validators;
    }

    public override async Task<Result> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var permissionCheckResult = await base.Handle(request, cancellationToken);

        if (permissionCheckResult.IsFailure || !_validators.Any())
            return permissionCheckResult;

        var validationResult = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(request, cancellationToken)));

        var validationErrors = validationResult.Where(x => !x.IsValid).ToArray();

        if (validationErrors.Length == 0)
            return permissionCheckResult;

        return new Result(false, string.Join(' ', validationErrors.Select(x => x.Errors.SelectMany(x => x.ErrorMessage))));
    }
}
