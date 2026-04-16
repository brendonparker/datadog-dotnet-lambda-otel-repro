using System.Diagnostics;
using DemoApi;

const string TenantId = "tenant.id";

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureOpenTelemetry();

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

app.MapGet("/", async () =>
{
    var isCurrentActivityIsNull = Activity.Current is null;
    var tenantId = "acme_inc";
    // This does not show - but I think it should.
    Activity.Current?.SetTag(TenantId, tenantId);
    using (var _ = Tracing.Source.StartActivity("CustomOTELSpan"))
    {
        // This does show - since this is a custom span
        Activity.Current?.SetTag(TenantId, tenantId);
        await Task.Delay(200);
    }

    return Results.Ok(new
    {
        isCurrentActivityIsNull,
        tenantId
    });
});

app.Run();