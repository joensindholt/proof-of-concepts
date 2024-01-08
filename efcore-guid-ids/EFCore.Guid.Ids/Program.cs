using System.Collections.Frozen;
using EFCore.Guid.Ids;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseInMemoryDatabase("MyDatabase"));

IHost host = builder.Build();
host.Run();

internal class Worker(DatabaseContext db) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Fii");

        db.Add(new User { Name = "John" });
        await db.SaveChangesAsync();

        db.Add(new User { Name = "Jim" });
        await db.SaveChangesAsync();

        db.Add(new User { Name = "Jane" });
        await db.SaveChangesAsync();

        db.Add(new User { Name = "Lis" });
        await db.SaveChangesAsync();

        db.Add(new User { Name = "Peter" });
        await db.SaveChangesAsync();

        var users = await db.Users.ToListAsync();

        Console.WriteLine("Users:");
        users.ForEach(u =>
        {
            Console.WriteLine($"Id: {u.Id}, Name: {u.Name}");
        });

        Console.WriteLine("Users sorted by id:");
        users.OrderBy(u => u.Id).ToList().ForEach(u =>
        {
            Console.WriteLine($"Id: {u.Id}, Name: {u.Name}");
        });


        await Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
