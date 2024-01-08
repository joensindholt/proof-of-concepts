var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/file", async (HttpRequest request) =>
{
    using var fileStream = File.OpenWrite("c:\\tmp\\outfile.mpeg");
    await request.Body.CopyToAsync(fileStream);
})
.WithName("File upload")
.WithOpenApi();

app.Run();