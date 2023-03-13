using System.Diagnostics.Metrics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Nexus.WeightTracker.Api.Infrastructure.Extensions;

public static class OpenTelemetryExtensions
{
    public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder builder)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName);
        var meter = new Meter(builder.Environment.ApplicationName);

        builder.Logging.AddOpenTelemetry(o =>
        {
            // TODO: Setup an exporter here
            o.SetResourceBuilder(resourceBuilder);
        });

        builder.Services.AddOpenTelemetry().WithTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .AddConsoleExporter()
                .AddZipkinExporter(options =>
                {
                    if (!string.IsNullOrEmpty(builder.Configuration.GetConnectionString("Zipkin")))
                    {
                        options.Endpoint = new Uri(builder.Configuration.GetConnectionString("Zipkin")!);
                    }
                })
                .AddSource(builder.Environment.ApplicationName)
                .SetResourceBuilder(resourceBuilder)
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation();
        });

        builder.Services.AddOpenTelemetry().WithMetrics(metricProviderBuilder =>
        {
            metricProviderBuilder
                .AddConsoleExporter()
                .AddMeter(meter.Name)
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddEventCountersInstrumentation(c =>
                {
                    // https://learn.microsoft.com/en-us/dotnet/core/diagnostics/available-counters
                    c.AddEventSources(
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft-AspNetCore-Server-Kestrel",
                        "System.Net.Http",
                        "System.Net.Sockets",
                        "System.Net.NameResolution",
                        "System.Net.Security");
                });
        });

        return builder;
    }
}
