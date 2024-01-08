using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder();

        builder.ConfigureServices(s =>
        {
            s.AddSingleton<MyWorker>();
        });

        var app = builder.Build();

        await app.Services.GetRequiredService<MyWorker>().Run();
    }
}

internal class MyWorker
{
    public async Task Run()
    {
        using var stream = File.OpenRead("C:\\Users\\joens\\Videos\\WIN_20231128_12_57_36_Pro.mp4");
        using var httpClient = new HttpClient();

        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5115/file")
        {
            Content = new StreamContent(stream)
        };

        await httpClient.SendAsync(request);
    }
}