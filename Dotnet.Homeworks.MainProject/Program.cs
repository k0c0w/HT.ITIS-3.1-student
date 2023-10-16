using Dotnet.Homeworks.MainProject.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Dotnet.Homeworks.MainProject.ServicesExtensions.DataAccess;
using Dotnet.Homeworks.MainProject.ServicesExtensions.CQRS;
using Dotnet.Homeworks.MainProject.Configuration;
using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.MainProject.ServicesExtensions.Masstransit;

var builder = WebApplication.CreateBuilder(args);
var rabbitMQconfig = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMqConfig>()!;

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.RegisterDataAccessServices(builder.Configuration);
builder.Services.AddCommandsAndQueries();

builder.Services.AddSingleton<IRegistrationService, RegistrationService>();
builder.Services.AddSingleton<ICommunicationService, CommunicationService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
builder.Services.AddAuthorization();

builder.Services.AddMasstransitRabbitMq(rabbitMQconfig);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();