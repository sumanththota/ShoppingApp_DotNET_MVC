using Microsoft.EntityFrameworkCore;
using ShoppingApp.Models;

namespace ShoppingApp.Data;

public class AppDbContext :DbContext
{
    //initializing the constructor
    public AppDbContext(DbContextOptions<AppDbContext> options) //enables the configurations for the context
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(p => p.Category)
            .HasConversion<string>();
    }
}