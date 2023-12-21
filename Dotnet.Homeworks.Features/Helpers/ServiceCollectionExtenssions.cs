using Dotnet.Homeworks.Features.PipelineBehaviors;
using Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.DependencyInjectionExtensions;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser.Services;
using Mapster;
using System.Reflection;

namespace Dotnet.Homeworks.Features.Helpers;

public static class ServiceCollectionExtensions
{
    public static void AddFeaturesDependencies(this IServiceCollection services)
    {
        services.AddMediator(AssemblyReference.Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AdminPermissionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly, ServiceLifetime.Transient);
        services.AddPermissionChecks(AssemblyReference.Assembly);

        services.AddSingleton<IRegistrationService, RegistrationService>();
        services.AddSingleton<ICommunicationService, CommunicationService>();

        services.AddMappersFromAssembly(AssemblyReference.Assembly);
    }

    public static IServiceCollection AddMappersFromAssembly(this IServiceCollection services, Assembly mapperConfigsAssembly)
    {
        var assemblyTypes = mapperConfigsAssembly.GetTypes();

        var markedInterfaces = assemblyTypes
            .Where(type => type.GetCustomAttribute<MapperAttribute>() is not null)
            .Where(type => type.IsInterface)
            .Where(type => !type.IsGenericType)
            .ToArray();

        var implementations = assemblyTypes
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .Select(type => new { ImplimitationCandidate = type, Interfaces = type.GetInterfaces().Intersect(markedInterfaces) })
            .Where(typeInfo => typeInfo.Interfaces.Any())
            .SelectMany(typeInfo => typeInfo.Interfaces.Select(@interface => new { Type = typeInfo.ImplimitationCandidate, MapperInterface = @interface }));

        foreach (var implInfo in implementations)
            services.AddSingleton(implInfo.MapperInterface, implInfo.Type);

        return services;
    }
}
