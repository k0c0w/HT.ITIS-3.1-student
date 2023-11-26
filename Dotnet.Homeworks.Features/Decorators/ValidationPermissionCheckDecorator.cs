using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Decorators;

public class ValidationPermissionCheckDecorator<TRequest, TResponse> : PermissionCheckDecorator<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPermissionCheckDecorator(IEnumerable<IValidator<TRequest>> validators, IPermissionCheck<IClientRequest>? permissionCheck) : base(permissionCheck)
    {
        _validators = validators;
    }

    public override async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var permissionCheckResult = await base.Handle(request, cancellationToken);

        if (permissionCheckResult is not Result result)
            throw new Exception();

        if (result!.IsFailure)
        {
            return result.Error as dynamic;
        }

        if (!_validators.Any())
        {
            return true as dynamic;
        }

        var validationResult = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(request, cancellationToken)));

        var validationErrors = validationResult.Where(x => !x.IsValid).ToArray();

        if (validationErrors.Length == 0)
        {
            return true as dynamic;
        }

        return string.Join(' ', validationErrors.Select(x => x.Errors.SelectMany(x => x.ErrorMessage))) as dynamic;
    }
}