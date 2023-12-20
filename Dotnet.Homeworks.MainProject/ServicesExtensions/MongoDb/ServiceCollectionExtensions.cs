using Dotnet.Homeworks.MainProject.Configuration;
using MongoDB.Driver;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.MongoDb;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoClient(this IServiceCollection services,
        MongoDbConfig mongoConfiguration)
    {
        services.AddSingleton<IMongoClient>(sp =>
        {
            return new MongoClient(mongoConfiguration.ConnectionString);
        });

        return services;
    }
}