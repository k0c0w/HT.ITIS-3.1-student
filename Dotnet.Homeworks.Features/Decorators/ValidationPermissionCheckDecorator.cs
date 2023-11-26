using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Decorators;

public class ValidationPermissionCheckDecorator<TRequest, TResponse> : PermissionCheckDecorator<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPermissionCheckDecorator(IEnumerable<IValidator<TRequest>> validators, IPermissionCheck<TRequest>? permissionCheck) : base(permissionCheck)
    {
        _validators = validators;
    }

    public override async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        dynamic permissionCheckResult = await base.Handle(request, cancellationToken);

        if (permissionCheckResult!.IsFailure || !_validators.Any())
            return permissionCheckResult;

        var validationResult = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(request, cancellationToken)));

        var validationErrors = validationResult.Where(x => !x.IsValid).ToArray();

        if (validationErrors.Length == 0)
            return permissionCheckResult;

        return new Result<TResponse>(default, false, string.Join(' ', validationErrors.Select(x => x.Errors.SelectMany(x => x.ErrorMessage)))) as dynamic;
    }
}