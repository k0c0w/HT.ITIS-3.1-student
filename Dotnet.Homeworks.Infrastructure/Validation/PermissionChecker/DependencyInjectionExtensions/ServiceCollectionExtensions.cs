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
        var permissionCheckType = typeof(PermissionCheck<>);
        var iPermissionCheckType = typeof(IPermissionCheck<>);

        foreach (var assembly in assemblies)
        {
            (Type InterfaceImplementerType, Type RequestType)[] values = assembly.GetTypes()
                .Select(x =>
                (
                    ImplemeterType: x,
                    IPermissionCheckType: x.GetInterfaces()
                        .FirstOrDefault(i => !(i.IsGenericTypeDefinition || i.ContainsGenericParameters) && i == iPermissionCheckType)
                ))
                .Where(x => x.IPermissionCheckType is not null)
                .Select(x => 
                (
                    InterfaceImplementerType: x.ImplemeterType, 
                    RequestType: x.IPermissionCheckType!.GetGenericArguments().First()
                ))
                .ToArray();

            foreach ((Type InterfaceImplementerType, Type RequestType) in values)
            {
                serviceCollection.AddTransient(iPermissionCheckType.MakeGenericType(RequestType), InterfaceImplementerType);
            }
        }
    }
}