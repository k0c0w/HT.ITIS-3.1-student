using Dotnet.Homeworks.Mailing.API.Configuration;
using MassTransit;

namespace Dotnet.Homeworks.Mailing.API.ServicesExtensions;

public static class AddMasstransitRabbitMqExtensions
{
    public static IServiceCollection AddMasstransitRabbitMq(this IServiceCollection services,
        RabbitMqConfig rabbitConfiguration)
    {
        services.AddMassTransit(options =>
        {
            var host = $"amqp://{rabbitConfiguration.Username}:{rabbitConfiguration.Password}@{rabbitConfiguration.Hostname}:{rabbitConfiguration.Port}";

            options.UsingRabbitMq((context, configuration) =>
            {
                configuration.ConfigureEndpoints(context);
                configuration.Host(host);
            });
        });

        return services;
    }
}