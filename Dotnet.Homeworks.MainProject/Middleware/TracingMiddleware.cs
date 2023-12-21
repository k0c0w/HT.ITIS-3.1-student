using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using OpenTelemetry.Trace;
namespace Dotnet.Homeworks.MainProject.Middleware;

public class TracingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ActivitySource activitySource;

    public TracingMiddleware(RequestDelegate next, Helpers.Instrumentation instrumentation)
    {
        _next = next;
        activitySource = instrumentation.ActivitySource;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using var activity = activitySource.StartActivity("Request Proccessing");

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            throw;
        }
    }
}
