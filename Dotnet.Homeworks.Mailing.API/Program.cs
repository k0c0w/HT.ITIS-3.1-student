using Dotnet.Homeworks.Mailing.API.Configuration;
using Dotnet.Homeworks.Mailing.API.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfig"));

builder.Services.AddMassTransit(options =>
{
    var config = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMqConfig>()!;
    var host = $"amqp://{config.Username}:{config.Password}@{config.Hostname}:{config.Port}";

    options.UsingRabbitMq((context, configuration) =>
    {
        configuration.ConfigureEndpoints(context);
        configuration.Host(host);
    });
});


builder.Services.AddScoped<IMailingService, MailingService>();

var app = builder.Build();

app.Run();