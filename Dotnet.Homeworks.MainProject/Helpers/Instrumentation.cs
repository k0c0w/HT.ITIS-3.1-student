using System.Diagnostics.Metrics;
using System.Diagnostics;

namespace Dotnet.Homeworks.MainProject.Helpers;

public class Instrumentation : IDisposable
{
    internal const string ActivitySourceName = "MainProject.MyTrace";
    internal const string MeterName = "MainProject.MyCounter";
    private readonly Meter meter;

    public Instrumentation()
    {
        string? version = AssemblyReference.Assembly.GetName().Version?.ToString();
        this.ActivitySource = new ActivitySource(ActivitySourceName, version);
        this.meter = new Meter(MeterName, version);
        this.RequestCounter = this.meter.CreateCounter<long>("mainAPI.requests.count", description: "The number of requests");
    }

    public ActivitySource ActivitySource { get; }

    public Counter<long> RequestCounter { get; }

    public void Dispose()
    {
        this.ActivitySource.Dispose();
        this.meter.Dispose();
    }
}