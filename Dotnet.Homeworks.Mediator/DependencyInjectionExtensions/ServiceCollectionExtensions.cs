using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] handlersAssemblies)
    {
        Assembly assembly = handlersAssemblies[0];

        var assemblyTypes = assembly.GetTypes();

        var assemblyClassesWithGenericInterfaces = assemblyTypes
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .Select(type => (Type: type, Interfaces: type.GetInterfaces().Where(i => i.IsGenericType).ToArray()))
            .Where(tuple => tuple.Interfaces.Any())
            .ToArray();

        RegisterHandler(services, typeof(IRequestHandler<,>), assemblyClassesWithGenericInterfaces);
        RegisterHandler(services, typeof(IPipelineBehavior<,>), assemblyClassesWithGenericInterfaces);

        var map = assemblyClassesWithGenericInterfaces
            .Select(tuple => (Type: tuple.Item1, CommandHandlerInterfaces: tuple.Item2.Where(i => i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)).ToArray()))
            .Where(tuple => tuple.CommandHandlerInterfaces.Any())
            .ToDictionary(key => key.Type.GetGenericArguments()[0], value => value.Type);


        services.AddSingleton(new MediatorRequestToHandlerMap(map));
        services.AddSingleton<IMediator, Mediator>();

        return services;
    }

    private static void RegisterHandler(IServiceCollection services, Type handlerType, (Type, Type[])[] typesWithInterfaces)
    {
        var handlers = typesWithInterfaces
               .Select(tuple => (Type: tuple.Item1, CommandHandlerInterfaces: tuple.Item2.Where(i => i.GetGenericTypeDefinition() == handlerType).ToArray()))
               .Where(tuple => tuple.CommandHandlerInterfaces.Any())
               .ToArray();

        foreach (var handlerInfo in handlers)
            foreach (var queryInterface in handlerInfo.CommandHandlerInterfaces)
                services.AddScoped(queryInterface, handlerInfo.Type);
    }
}