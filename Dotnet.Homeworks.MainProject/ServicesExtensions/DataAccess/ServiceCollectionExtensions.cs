using Dotnet.Homeworks.DataAccess.Repositories;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.DataAccess;

public static partial class ServiceCollectionExtensions
{
    public static void RegisterDataAccessServices(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();
    }
}