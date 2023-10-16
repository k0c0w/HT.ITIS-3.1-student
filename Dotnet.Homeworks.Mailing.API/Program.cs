using Dotnet.Homeworks.Mailing.API.Configuration;
using Dotnet.Homeworks.Mailing.API.Services;
using Dotnet.Homeworks.Mailing.API.ServicesExtensions;

var builder = WebApplication.CreateBuilder(args);
var rabbitMQconfig = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMqConfig>()!;

builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfig"));

builder.Services.AddMasstransitRabbitMq(rabbitMQconfig);
builder.Services.AddScoped<IMailingService, MailingService>();

var app = builder.Build();

app.Run();