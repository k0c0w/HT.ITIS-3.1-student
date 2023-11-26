using Dotnet.Homeworks.DataAccess.Repositories;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.PipelineBehaviors;
using Dotnet.Homeworks.Features.Users.Commands.DeleteUser;
using Dotnet.Homeworks.Features.Users.Commands.UpdateUser;
using Dotnet.Homeworks.Features.Users.Queries.GetUser;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser;
using Dotnet.Homeworks.Features.Decorators;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.DependencyInjectionExtensions;

namespace Dotnet.Homeworks.Features.Helpers;

public static class ServiceCollectionExtenssions
{
    public static void AddFeaturesDependencies(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IProductRepository, ProductRepository>();

        services.AddMediator(AssemblyReference.Assembly);
        
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly, ServiceLifetime.Transient);

        services.AddPermissionChecks(AssemblyReference.Assembly);
    }

    private static void AddCqrsDecorators(IServiceCollection services)
    {
        services.AddTransient<IRequestHandler<UpdateUserCommand, Result>, CqrsDecorator<UpdateUserCommand, Result>>();
        services.AddTransient<IRequestHandler<DeleteUserCommand, Result>, CqrsDecorator<DeleteUserCommand, Result>>();
        services.AddTransient<IRequestHandler<GetUserQuery, Result<GetUserDto>>, CqrsDecorator<GetUserQuery, Result<GetUserDto>>>();
        services.AddTransient<IRequestHandler<CreateUserCommand, Result<CreateUserDto>>, CqrsDecorator<CreateUserCommand, Result<CreateUserDto>>>();
    }
}
