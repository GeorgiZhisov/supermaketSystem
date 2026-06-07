using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using SupermarketManagerSystem.Data;
using SupermarketManagerSystem.Data.Models;
using SupermarketManagerSystem.Services.Implementations;
using SupermarketManagerSystem.Services.Models;

namespace SupermarketManagerSystem.Tests;

[TestFixture]
public class ProductServiceTests
{
    private ApplicationDbContext _context = null!;
    private ProductService _productService = null!;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        // Seed a category
        _context.Categories.Add(new Category { Id = 1, Name = "Test Category" });
        _context.SaveChanges();

        _productService = new ProductService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetAllAsync_ReturnsOnlyActiveProducts_WhenActiveOnlyTrue()
    {
        // Arrange
        _context.Products.AddRange(
            new Product { Name = "Active Product", Barcode = "111", Price = 1.99m, IsActive = true, CategoryId = 1 },
            new Product { Name = "Inactive Product", Barcode = "222", Price = 2.99m, IsActive = false, CategoryId = 1 }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _productService.GetAllAsync(activeOnly: true);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().Name, Is.EqualTo("Active Product"));
    }

    [Test]
    public async Task GetByIdAsync_ReturnsProduct_WhenExists()
    {
        // Arrange
        _context.Products.Add(new Product { Id = 10, Name = "Test", Barcode = "999", Price = 5m, IsActive = true, CategoryId = 1 });
        await _context.SaveChangesAsync();

        // Act
        var result = await _productService.GetByIdAsync(10);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("Test"));
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        var result = await _productService.GetByIdAsync(9999);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateAsync_AddsProductToDatabase()
    {
        // Arrange
        var model = new ProductServiceModel
        {
            Name = "New Product", Barcode = "BAR001", Price = 3.99m, CostPrice = 2.00m,
            StockQuantity = 50, MinimumStockLevel = 10, IsActive = true, CategoryId = 1
        };

        // Act
        var id = await _productService.CreateAsync(model);

        // Assert
        Assert.That(id, Is.GreaterThan(0));
        var savedProduct = await _context.Products.FindAsync(id);
        Assert.That(savedProduct, Is.Not.Null);
        Assert.That(savedProduct!.Name, Is.EqualTo("New Product"));
    }

    [Test]
    public async Task UpdateAsync_ReturnsFalse_WhenProductNotFound()
    {
        var model = new ProductServiceModel { Id = 9999, Name = "Ghost", Barcode = "X", Price = 1m, CategoryId = 1 };
        var result = await _productService.UpdateAsync(model);
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task UpdateAsync_UpdatesProduct_WhenExists()
    {
        // Arrange
        var product = new Product { Name = "Old Name", Barcode = "BAR", Price = 1m, IsActive = true, CategoryId = 1 };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var model = new ProductServiceModel
        {
            Id = product.Id, Name = "New Name", Barcode = "BAR", Price = 2m,
            IsActive = true, CategoryId = 1
        };

        // Act
        var result = await _productService.UpdateAsync(model);

        // Assert
        Assert.That(result, Is.True);
        var updated = await _context.Products.FindAsync(product.Id);
        Assert.That(updated!.Name, Is.EqualTo("New Name"));
    }

    [Test]
    public async Task DeleteAsync_DeactivatesProduct_WhenExists()
    {
        var product = new Product { Name = "To Delete", Barcode = "DEL", Price = 1m, IsActive = true, CategoryId = 1 };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var result = await _productService.DeleteAsync(product.Id);

        Assert.That(result, Is.True);
        var deactivated = await _context.Products.FindAsync(product.Id);
        Assert.That(deactivated!.IsActive, Is.False);
    }

    [Test]
    public async Task GetLowStockProductsAsync_ReturnsProductsBelowMinimum()
    {
        _context.Products.AddRange(
            new Product { Name = "Low Stock", Barcode = "L1", Price = 1m, StockQuantity = 3, MinimumStockLevel = 10, IsActive = true, CategoryId = 1 },
            new Product { Name = "Good Stock", Barcode = "L2", Price = 1m, StockQuantity = 50, MinimumStockLevel = 10, IsActive = true, CategoryId = 1 }
        );
        await _context.SaveChangesAsync();

        var result = await _productService.GetLowStockProductsAsync();

        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().Name, Is.EqualTo("Low Stock"));
    }

    [Test]
    public async Task UpdateStockAsync_IncreasesStock_WhenPositiveQuantity()
    {
        var product = new Product { Name = "Stock Test", Barcode = "ST1", Price = 1m, StockQuantity = 10, IsActive = true, CategoryId = 1 };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var result = await _productService.UpdateStockAsync(product.Id, 20);

        Assert.That(result, Is.True);
        var updated = await _context.Products.FindAsync(product.Id);
        Assert.That(updated!.StockQuantity, Is.EqualTo(30));
    }

    [Test]
    public async Task ExistsAsync_ReturnsTrue_WhenProductExists()
    {
        var product = new Product { Name = "Exists", Barcode = "EX1", Price = 1m, IsActive = true, CategoryId = 1 };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var result = await _productService.ExistsAsync(product.Id);
        Assert.That(result, Is.True);
    }
}
