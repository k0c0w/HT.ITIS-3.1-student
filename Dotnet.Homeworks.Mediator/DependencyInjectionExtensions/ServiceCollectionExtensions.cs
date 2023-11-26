using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] handlersAssemblies)
    {
        var scanningTypes = handlersAssemblies
            .SelectMany(x => x.GetTypes())
            .ToArray();
        var requestHandlersInfos = GetRequestHandlerImplementationInfos(scanningTypes);

        RegisterIPipelineBehaviorImplementationTypes(services, scanningTypes);
        RegisterIRequestHandlersImplementationTypes(services, requestHandlersInfos);

        services.AddSingleton(CreateMapForMediator(requestHandlersInfos));
        services.AddSingleton<IMediator, Mediator>();

        return services;
    }

    private static void RegisterIPipelineBehaviorImplementationTypes(IServiceCollection services, IEnumerable<Type> scanningTypes)
    {
        var iPipelineBehaviorType = typeof(IPipelineBehavior<,>);
        var implementations = scanningTypes
            .Where(type => type.GetInterfaces().Any(interfaceType =>
                interfaceType.IsGenericType &&
                interfaceType.GetGenericTypeDefinition() == iPipelineBehaviorType));

        foreach (var implementationType in implementations)
            services.TryAddTransient(iPipelineBehaviorType, implementationType);
    }

    private static void RegisterIRequestHandlersImplementationTypes(IServiceCollection services, IEnumerable<RequestHandlerImplementationInfo> requestHandlersWithImplementingIRequestHandlerInterfaces)
    {
        foreach (var implementationWithImplementingInterfaces in requestHandlersWithImplementingIRequestHandlerInterfaces)
        {
            var implementation = implementationWithImplementingInterfaces.ImplementationType;
            foreach (var service in implementationWithImplementingInterfaces.ConcreteRequestHandlerInterfaces)
                services.TryAddTransient(service, implementation);
        }
    }

    private static RequestHandlerImplementationInfo[] GetRequestHandlerImplementationInfos(IEnumerable<Type> scanningTypes)
    {
        var iRequestHandlerType = typeof(IRequestHandler<>);
        var iRequestHandler2Type = typeof(IRequestHandler<,>);
        var concreteTypes = scanningTypes
            .Where(IsConcrete)
            .ToArray();

        var typesWithIRequestHandler2Interfaces = concreteTypes
            .Select(type => new RequestHandlerImplementationInfo
            {
                ImplementationType = type,
                ConcreteRequestHandlerInterfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == iRequestHandler2Type)
                    .Where(i => !i.IsOpenGeneric())
                    .ToArray()
            })
            .Where(info => info.ConcreteRequestHandlerInterfaces.Any());

        var typesWithIRequestHandlerInterfaces = concreteTypes
            .Select(type => new RequestHandlerImplementationInfo
            {
                ImplementationType = type,
                ConcreteRequestHandlerInterfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == iRequestHandlerType)
                    .Where(i => !i.IsOpenGeneric())
                    .ToArray()
            })
            .Where(info => info.ConcreteRequestHandlerInterfaces.Any());

        return typesWithIRequestHandlerInterfaces
            .Concat(typesWithIRequestHandler2Interfaces)
            .ToArray();
    }

    private static MediatorRequestToHandlerMap CreateMapForMediator(IEnumerable<RequestHandlerImplementationInfo> infos)
    {
        var map = infos
            .SelectMany(info => info.ConcreteRequestHandlerInterfaces.Select(i => new
            {
                RequestType = i.GetGenericArguments().First(),
                HadlerInterfaceType = i
            }))
            .ToDictionary(key => key.RequestType, value => value.HadlerInterfaceType);


        return new MediatorRequestToHandlerMap(map);
    }

    private static bool IsConcrete(this Type type)
    {
        return !type.IsAbstract && !type.IsInterface;
    }

    private static bool IsOpenGeneric(this Type type)
    {
        return type.IsGenericTypeDefinition || type.ContainsGenericParameters;
    }

    private record RequestHandlerImplementationInfo
    {
        public Type ImplementationType { get; init; }

        public IEnumerable<Type> ConcreteRequestHandlerInterfaces { get; init; }
    }
}