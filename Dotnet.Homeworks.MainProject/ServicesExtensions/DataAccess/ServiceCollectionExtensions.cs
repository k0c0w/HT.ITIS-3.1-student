using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.DataAccess.Repositories;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions;

public static partial class ServiceCollectionExtensions
{
    public static void RegisterDataAccessServices(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();
    }
}