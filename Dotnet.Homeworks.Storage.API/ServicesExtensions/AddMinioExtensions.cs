using Minio;

namespace Dotnet.Homeworks.Storage.API.ServicesExtensions;

public static class AddMinioExtensions
{
    public static IServiceCollection AddMinioClient(this IServiceCollection services,
        Configuration.MinioConfig minioConfiguration)
    {
        return services.AddMinio(client =>
        {
            client.WithSSL(minioConfiguration.WithSsl)
                  .WithEndpoint(minioConfiguration.Endpoint, minioConfiguration.Port)
                  .WithCredentials(minioConfiguration.Username, minioConfiguration.Password);
        }, ServiceLifetime.Singleton);
    }
}