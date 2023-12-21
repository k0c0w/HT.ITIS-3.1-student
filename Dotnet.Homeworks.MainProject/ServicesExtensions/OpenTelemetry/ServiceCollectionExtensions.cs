using Dotnet.Homeworks.MainProject.Configuration;
using OpenTelemetry.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Npgsql;
using System.Reflection.PortableExecutable;
using MassTransit.Monitoring;
using Dotnet.Homeworks.MainProject.Helpers;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.OpenTelemetry;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services,
        OpenTelemetryConfig openTelemetryConfiguration)
    {
        services.AddSingleton<Instrumentation>();

        Action<ResourceBuilder> configureResource = r => r.AddService(
            "MainProject",
            Helpers.AssemblyReference.Assembly.GetName().Version.ToString()!,
            serviceInstanceId: Environment.MachineName);

        services.AddOpenTelemetry()
            .ConfigureResource(configureResource)
            .WithTracing(tracerBuilder => tracerBuilder
                .AddAspNetCoreInstrumentation()
                .AddNpgsql()
                .AddHttpClientInstrumentation()
                .AddSource(Helpers.Instrumentation.ActivitySourceName)
                .SetSampler(new AlwaysOnSampler())
                .AddOtlpExporter(conf =>
                {
                    conf.Endpoint = new Uri(openTelemetryConfiguration.OtlpExporterEndpoint);
                }))
           .WithMetrics(metrics => metrics
                .AddMeter(Helpers.Instrumentation.MeterName)
                .AddAspNetCoreInstrumentation()
                .AddConsoleExporter());

        return services;
    }
}