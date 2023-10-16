using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.CQRS;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandsAndQueries(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(ICommand).Assembly);
        });

        return services;
    }
}