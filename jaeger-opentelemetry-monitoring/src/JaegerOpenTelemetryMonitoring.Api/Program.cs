using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSeq();

builder.Services.AddOpenTelemetry()
    .WithTracing(b => b
        .SetResourceBuilder(ResourceBuilder
            .CreateDefault()
            .AddService(builder.Environment.ApplicationName))
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter()
        .AddZipkinExporter(c =>
        {
            c.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
            c.ExportProcessorType = OpenTelemetry.ExportProcessorType.Simple;
        }));

builder.Services.AddHttpClient("httpstat", c => c.BaseAddress = new Uri("https://httpstat.us"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/hello", async () =>
{
    await Task.Delay(500);
    return TypedResults.Ok<string>("Hellog again");
});

app.MapGet("/external", async (IHttpClientFactory clientFactory) =>
{
    var client = clientFactory.CreateClient("httpstat");
    await client.GetStringAsync("200?sleep=1000");
    return TypedResults.Ok();
});

app.MapGet("/external/error", async (IHttpClientFactory clientFactory) =>
{
    await Task.Delay(500);

    var client = clientFactory.CreateClient("httpstat");
    await client.GetStringAsync("500?sleep=1000");

    return TypedResults.Ok();
});

app.MapGet("/error", () =>
{
    throw new ApplicationException("Something went wrong!!!");
});

app.Run();
