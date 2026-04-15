using DemoApi;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace DemoApi;

static class WebApplicationBuilderExtensions
{
    public static void ConfigureOpenTelemetry(this WebApplicationBuilder builder)
    {
        builder
            .Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSource(Tracing.Name);
            });
    }
}