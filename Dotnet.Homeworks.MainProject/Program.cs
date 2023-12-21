using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.MainProject.Configuration;
using Dotnet.Homeworks.MainProject.ServicesExtensions.Masstransit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Dotnet.Homeworks.Features.Helpers;
using Dotnet.Homeworks.MainProject.ServicesExtensions.MongoDb;
using Dotnet.Homeworks.MainProject.ServicesExtensions.DataAccess;
using Dotnet.Homeworks.MainProject.Middleware;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var rabbitMQConfig = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMqConfig>()!;
var mongoDBConfig = builder.Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>()!;

services.AddControllers();

services.AddFeaturesDependencies();

services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("Default")));
services.RegisterDataAccessServices(configuration);
services.AddMongoClient(mongoDBConfig);

services.AddMasstransitRabbitMq(rabbitMQConfig);

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
services.AddAuthorization();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<RequestCounterMiddleware>();
app.UseMiddleware<TracingMiddleware>();

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