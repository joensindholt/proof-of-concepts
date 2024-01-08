using Microsoft.EntityFrameworkCore;

namespace EFCore.Guid.Ids;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        base.OnModelCreating(modelBuilder);
    }
}

public class User
{
    public System.Guid? Id { get; set; }

    public string? Name { get; set; }
}