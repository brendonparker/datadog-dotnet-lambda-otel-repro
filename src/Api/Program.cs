using System.Diagnostics;
using DemoApi;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureOpenTelemetry();

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

app.MapGet("/", async (ILogger<Program> log) =>
{
    log.LogInformation("Activity.Current is null: {isNull}", Activity.Current == null);
    Activity.Current?.SetTag("tenant.id", "acme_inc");
    using (var _ = Tracing.Source.StartActivity("CustomOTELSpan"))
    {
        Activity.Current?.SetTag("tenant.id", "acme_inc");
        await Task.Delay(200);
    }

    return Results.Ok(new
    {
        currentActivityIsNull = Activity.Current == null,
        tenantId = "acme_inc"
    });
});

app.Run();