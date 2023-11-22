using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Dotnet.Homeworks.Features.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, Result<TResult>> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }


    public Task<Result<TResult>> Handle(TRequest request, RequestHandlerDelegate<Result<TResult>> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        if (_validators.Any())
        {
            var error = _validators.Select(x => x.Validate(request))
                .FirstOrDefault(x => !x.IsValid);

            if (error != null)
                return Task.FromResult(new Result<TResult>(default, false, error.Errors.FirstOrDefault()?.ErrorMessage));
        }

        return next();
    }
}
