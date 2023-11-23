using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Infrastructure.Validation.Decorators;

public class CqrsDecorator<TRequest, TResponse> : ValidationPermissionCheckDecorator<TRequest, TResponse> where TRequest : IRequest<Result<TResponse>>
{
    private readonly IRequestHandler<TRequest, Result<TResponse>> _decoratee;

    public CqrsDecorator(IRequestHandler<TRequest, Result<TResponse>> requestHandler, IEnumerable<IValidator<TRequest>> validators, IPermissionCheck<TRequest>? permissionCheck) 
        : base(validators, permissionCheck)
    {
        _decoratee = requestHandler;
    }

    public override async Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var pipelineResult = await Handle(request, cancellationToken);

        return pipelineResult.IsFailure 
            ? pipelineResult
            : await _decoratee.Handle(request, cancellationToken);
    }
}

public class CqrsDecorator<TRequest> : ValidationPermissionCheckDecorator<TRequest> where TRequest : IRequest<Result>
{
    private readonly IRequestHandler<TRequest, Result> _decoratee;

    public CqrsDecorator(IRequestHandler<TRequest, Result> requestHandler, IEnumerable<IValidator<TRequest>> validators, IPermissionCheck<TRequest>? permissionCheck)
        : base(validators, permissionCheck)
    {
        _decoratee = requestHandler;
    }

    public override async Task<Result> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var pipelineResult = await Handle(request, cancellationToken);

        return pipelineResult.IsFailure
            ? pipelineResult
            : await _decoratee.Handle(request, cancellationToken);
    }
}