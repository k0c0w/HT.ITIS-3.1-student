using System.Reflection;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddMappers(this IServiceCollection services, Assembly mapperConfigsAssembly)
    {
        // TODO: добавить автоматическую регистрацию всех мапперов, найденных в переданной сборке, с помощью рефлексии
        throw new NotImplementedException();
    }
}