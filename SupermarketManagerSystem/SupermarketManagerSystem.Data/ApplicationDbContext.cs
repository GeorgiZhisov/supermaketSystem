using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SupermarketManagerSystem.Data.Models;

namespace SupermarketManagerSystem.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();
    public DbSet<RestockRequest> RestockRequests => Set<RestockRequest>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>(entity =>
        {
            entity.HasOne(p => p.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Supplier)
                  .WithMany(s => s.Products)
                  .HasForeignKey(p => p.SupplierId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(p => p.Barcode).IsUnique();
        });

        builder.Entity<StockTransaction>(entity =>
        {
            entity.HasOne(st => st.Product)
                  .WithMany(p => p.StockTransactions)
                  .HasForeignKey(st => st.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<RestockRequest>(entity =>
        {
            entity.HasOne(rr => rr.Product)
                  .WithMany(p => p.RestockRequests)
                  .HasForeignKey(rr => rr.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(rr => rr.Supplier)
                  .WithMany(s => s.RestockRequests)
                  .HasForeignKey(rr => rr.SupplierId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Seed Categories
        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Fruits & Vegetables", Description = "Fresh produce" },
            new Category { Id = 2, Name = "Dairy & Eggs", Description = "Milk, cheese, eggs and more" },
            new Category { Id = 3, Name = "Meat & Poultry", Description = "Fresh and frozen meats" },
            new Category { Id = 4, Name = "Bakery", Description = "Bread, pastries and baked goods" },
            new Category { Id = 5, Name = "Beverages", Description = "Drinks, juices and water" },
            new Category { Id = 6, Name = "Snacks & Confectionery", Description = "Chips, chocolates and candy" },
            new Category { Id = 7, Name = "Frozen Foods", Description = "Frozen meals and vegetables" },
            new Category { Id = 8, Name = "Household & Cleaning", Description = "Cleaning supplies and household items" }
        );

        // Seed Suppliers
        builder.Entity<Supplier>().HasData(
            new Supplier { Id = 1, Name = "FreshFarm Ltd.", ContactPerson = "Ivan Petrov", Email = "ivan@freshfarm.bg", Phone = "+359888100001", Address = "Sofia, Mladost 1", IsActive = true },
            new Supplier { Id = 2, Name = "DairyPro EOOD", ContactPerson = "Maria Ivanova", Email = "maria@dairypro.bg", Phone = "+359888100002", Address = "Plovdiv, Industrial Zone", IsActive = true },
            new Supplier { Id = 3, Name = "MeatMasters AD", ContactPerson = "Georgi Georgiev", Email = "georgi@meatmasters.bg", Phone = "+359888100003", Address = "Varna, Port Area", IsActive = true }
        );
    }
}
