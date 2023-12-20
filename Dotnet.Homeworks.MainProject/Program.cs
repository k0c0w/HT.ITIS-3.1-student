using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.MainProject.Configuration;
using Dotnet.Homeworks.MainProject.ServicesExtensions.Masstransit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Dotnet.Homeworks.Features.Helpers;
using Dotnet.Homeworks.MainProject.ServicesExtensions.MongoDb;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var rabbitMQConfig = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMqConfig>()!;
var mongoDBConfig = builder.Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>()!;

services.AddControllers();

services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("Default")));

services.AddFeaturesDependencies();

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
services.AddAuthorization();

services.AddMasstransitRabbitMq(rabbitMQConfig);
services.AddMongoClient(mongoDBConfig);


services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

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