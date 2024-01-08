using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkDddEntities;

public class DatabaseContext : DbContext
{
    public string DbPath { get; }

    public DbSet<Customer> Customers { get; set; }

    public DatabaseContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "blogging.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>()
            .Property(c => c.Name)
            .HasMaxLength(255);

        base.OnModelCreating(modelBuilder);
    }
}
