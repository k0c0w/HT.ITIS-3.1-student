using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Decorators;

public class ValidationkDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationkDecorator(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return new Result<TResponse>(default, true) as dynamic;

        var validationResult = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(request, cancellationToken)));

        var validationErrors = validationResult.Where(x => !x.IsValid).ToArray();

        if (validationErrors.Length == 0)
            return new Result<TResponse>(default, true) as dynamic;

        return new Result<TResponse>(default, false, string.Join(' ', validationErrors.Select(x => x.Errors.SelectMany(x => x.ErrorMessage)))) as dynamic;
    }
}