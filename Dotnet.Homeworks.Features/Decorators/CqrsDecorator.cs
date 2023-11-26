using Dotnet.Homeworks.Infrastructure.Validation.Decorators;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Mediator;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Decorators;


public class CqrsDecorator<TRequest, TResponse> : ValidationPermissionCheckDecorator<TRequest, TResponse> where TRequest : IRequest<TResponse>
{

    public CqrsDecorator(IEnumerable<IValidator<TRequest>> validators, IPermissionCheck<TRequest>? permissionCheck)
        : base(validators, permissionCheck)
    {
    }

    public override async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var pipelineResult = await Handle(request, cancellationToken);

        return pipelineResult;
    }
}