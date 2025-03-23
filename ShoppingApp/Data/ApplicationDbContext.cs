using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Models;

namespace ShoppingApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Add your application-specific DbSets
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Cart> Carts { get; set; } = default!;
    public DbSet<CartItem> CartItems { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Call base to ensure Identity tables are configured
        base.OnModelCreating(modelBuilder);

        // Configure your domain-specific model
        modelBuilder.Entity<Product>()
            .Property(p => p.Category)
            .HasConversion<string>();
    }
}