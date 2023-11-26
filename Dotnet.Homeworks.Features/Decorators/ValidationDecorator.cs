using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Decorators;

public class ValidationDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationDecorator(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            if (typeof(TResponse) == typeof(Result))
                return new Result(true) as dynamic;
            else
                return new Result<TResponse>(default, true) as dynamic;
        }

        var validationResult = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(request, cancellationToken)));

        var validationErrors = validationResult.Where(x => !x.IsValid).ToArray();

        if (validationErrors.Length == 0)
        {
            if (typeof(TResponse) == typeof(Result))
                return new Result(true) as dynamic;
            else
                return new Result<TResponse>(default, true) as dynamic;
        }

        if (typeof(TResponse) == typeof(Result))
            return new Result(false, string.Join(' ', validationErrors.Select(x => x.Errors.SelectMany(x => x.ErrorMessage)))) as dynamic;
        else
            return new Result<TResponse>(default, false, string.Join(' ', validationErrors.Select(x => x.Errors.SelectMany(x => x.ErrorMessage)))) as dynamic;
    }
}