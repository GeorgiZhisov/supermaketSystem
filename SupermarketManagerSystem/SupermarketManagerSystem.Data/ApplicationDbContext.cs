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
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<StoreBranch> StoreBranches => Set<StoreBranch>();
}
