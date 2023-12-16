using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.DependencyInjectionExtensions;

public static class ServiceCollectionExtensions
{
    public static void AddPermissionChecks(
        this IServiceCollection serviceCollection,
        Assembly assembly
    )
    {
        AddPermissionChecks(serviceCollection, new Assembly[] { assembly });
    }
    
    public static void AddPermissionChecks(
        this IServiceCollection serviceCollection,
        Assembly[] assemblies
    )
    {
        var iPermissionCheckType = typeof(IPermissionCheck<>);

        var implementationsWithInterfaces = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(IsConcrete)
            .Select(type => new
            {
                ImplementationType = type,
                ConcretePermissionCheckInterfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == iPermissionCheckType)
                    .Where(i => !i.IsOpenGeneric())
                    .ToArray()
            })
            .Where(info => info.ConcretePermissionCheckInterfaces.Any());

        foreach (var info in implementationsWithInterfaces)
        {
            var implementation = info.ImplementationType;
            foreach (var permissionCheckInterfaceType in info.ConcretePermissionCheckInterfaces)
                serviceCollection.TryAddTransient(permissionCheckInterfaceType, implementation);
        }
    }

    private static bool IsConcrete(this Type type)
    {
        return !type.IsAbstract && !type.IsInterface;
    }

    private static bool IsOpenGeneric(this Type type)
    {
        return type.IsGenericTypeDefinition || type.ContainsGenericParameters;
    }
}