using System.Diagnostics.Metrics;

namespace Dotnet.Homeworks.MainProject.Middleware;

public class RequestCounterMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Counter<long> _counter;

    public RequestCounterMiddleware(RequestDelegate next, Helpers.Instrumentation instrumentation)
    {
        _next = next;
        _counter = instrumentation.RequestCounter;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _counter.Add(1, new  KeyValuePair<string, object?>("Method",  context.Request.Method.ToString()));
        await _next(context);
    }
}