using Dotnet.Homeworks.DataAccess.Repositories;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.PipelineBehaviors;
using Dotnet.Homeworks.Features.Users.Commands.DeleteUser;
using Dotnet.Homeworks.Features.Users.Commands.UpdateUser;
using Dotnet.Homeworks.Features.Users.Queries.GetUser;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.Decorators;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Features.Helpers;

public static class ServiceCollectionExtenssions
{
    public static void AddFeaturesDependencies(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IProductRepository, ProductRepository>();

        services.AddMediator(AssemblyReference.Assembly);
        
        services
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(AdminPermissionBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        AddCqrsDecorators(services);
    }

    private static void AddCqrsDecorators(IServiceCollection services)
    {
        services.AddTransient<IRequestHandler<UpdateUserCommand, Result>, CqrsDecorator<UpdateUserCommand>>();
        services.AddTransient<IRequestHandler<DeleteUserCommand, Result>, CqrsDecorator<DeleteUserCommand>>();
        services.AddTransient<IRequestHandler<GetUserQuery, Result<GetUserDto>>, CqrsDecorator<GetUserQuery, GetUserDto>>();
    }
}
