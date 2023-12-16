using Dotnet.Homeworks.Mediator;
using FluentValidation;

namespace Dotnet.Homeworks.Features.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResult> : IPipelineBehavior<TRequest,TResult>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }


    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        if (_validators.Any())
        {
            var error = _validators.Select(x => x.Validate(request))
                .FirstOrDefault(x => !x.IsValid);

            if (error != null)
                return error.Errors.FirstOrDefault()?.ErrorMessage as dynamic;
        }

        return await next();
    }
}