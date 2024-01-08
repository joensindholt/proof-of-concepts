using Cocona;
using EntityFrameworkDddEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddDbContext<DatabaseContext>();

var app = builder.Build();

app.AddCommand("hello", async (DatabaseContext db) =>
{
    await db.Database.EnsureDeletedAsync();
    await db.Database.EnsureCreatedAsync();

    await db.Customers.AddAsync(new Customer("Hans"));
    await db.Customers.AddAsync(new Customer("Line"));
    await db.SaveChangesAsync();

    var customers = await db.Customers.AsNoTracking().ToListAsync();
    foreach (var loadedCustomer in customers)
    {
        Console.WriteLine($"Id: {loadedCustomer.Id}, Name: {loadedCustomer.Name}");
    }
});

app.Run();