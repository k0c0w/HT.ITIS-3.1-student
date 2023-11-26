using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Mediator;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Decorators;


public class CqrsDecorator<TRequest, TResponse> : UserPermissionCheckValidationDecorator<TRequest, TResponse> where TRequest : IRequest<TResponse>
{

    public CqrsDecorator(IEnumerable<IValidator<TRequest>> validators, IPermissionCheck<IClientRequest> permissionCheck)
        : base(validators, permissionCheck)
    {
    }
}