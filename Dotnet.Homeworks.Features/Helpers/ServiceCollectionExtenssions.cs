using Dotnet.Homeworks.DataAccess.Repositories;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.PipelineBehaviors;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.DependencyInjectionExtensions;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser.Services;

namespace Dotnet.Homeworks.Features.Helpers;

public static class ServiceCollectionExtenssions
{
    public static void AddFeaturesDependencies(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IProductRepository, ProductRepository>();

        services.AddMediator(AssemblyReference.Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AdminPermissionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly, ServiceLifetime.Transient);
        services.AddPermissionChecks(AssemblyReference.Assembly);

        services.AddSingleton<IRegistrationService, RegistrationService>();
        services.AddSingleton<ICommunicationService, CommunicationService>();
    }
}
