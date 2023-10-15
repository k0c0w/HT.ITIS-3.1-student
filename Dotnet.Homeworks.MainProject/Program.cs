using Dotnet.Homeworks.MainProject.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Dotnet.Homeworks.MainProject.ServicesExtensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();


builder.Services.RegisterDataAccessServices(builder.Configuration);
builder.Services.AddCommandsAndQueries();

builder.Services.AddSingleton<IRegistrationService, RegistrationService>();
builder.Services.AddSingleton<ICommunicationService, CommunicationService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

app.MapControllers();

app.Run();