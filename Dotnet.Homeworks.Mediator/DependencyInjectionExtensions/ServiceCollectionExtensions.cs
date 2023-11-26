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

        RegisterHandler(services,typeof(IRequestHandler<,>), assemblyClassesWithGenericInterfaces);

        #region pipeline
        var handlers = assemblyClassesWithGenericInterfaces
               .Select(tuple => (Type: tuple.Item1, CommandHandlerInterfaces: tuple.Item2.Where(i => i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)).ToArray()))
               .Where(tuple => tuple.CommandHandlerInterfaces.Any())
               .ToArray();

        foreach (var handlerInfo in handlers)
            foreach (var queryInterface in handlerInfo.CommandHandlerInterfaces)
            {
                services.AddTransient(queryInterface, handlerInfo.Type);
            }
        #endregion




        var map = assemblyClassesWithGenericInterfaces
            .Select(tuple => (Type: tuple.Item1, CommandHandlerInterfaces: tuple.Item2.Where(i => i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)).ToArray()))
            .Where(tuple => tuple.CommandHandlerInterfaces.Any())
            .SelectMany(x => x.CommandHandlerInterfaces.Select(y => (x.Type, y.GetGenericArguments()[0])))
            .ToDictionary(key => key.Item2, value => value.Item1);


        services.AddSingleton(new MediatorRequestToHandlerMap(map));
        services.AddSingleton<IMediator, Mediator>();

        return services;
    }

    private static void RegisterHandler(IServiceCollection services, Type handlerType, (Type, Type[])[] typesWithInterfaces)
    {
        var handlers = typesWithInterfaces
       .Select(tuple => (Type: tuple.Item1, CommandHandlerInterfaces: tuple.Item2.Where(i => i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)).ToArray()))
       .Where(tuple => tuple.CommandHandlerInterfaces.Any())
       .ToArray();

        foreach (var handlerInfo in handlers)
            foreach (var queryInterface in handlerInfo.CommandHandlerInterfaces)
            {
                services.AddTransient(queryInterface, handlerInfo.Type);
            }
    }
}