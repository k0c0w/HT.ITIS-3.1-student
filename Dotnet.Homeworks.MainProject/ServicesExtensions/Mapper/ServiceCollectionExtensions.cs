using Dotnet.Homeworks.Features.Helpers;
using Mapster;
using System.Reflection;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.Mapper;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMappers(this IServiceCollection services, Assembly mapperConfigsAssembly)
    {
        return services.AddMappersFromAssembly(mapperConfigsAssembly);
    }
}