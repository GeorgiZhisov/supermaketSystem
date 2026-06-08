using Microsoft.EntityFrameworkCore;
using SupermarketManagerSystem.Data;
using SupermarketManagerSystem.Data.Models;
using SupermarketManagerSystem.Services.Contracts;
using SupermarketManagerSystem.Services.Models;

namespace SupermarketManagerSystem.Services.Implementations;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductServiceModel>> GetAllAsync(string? searchTerm = null, int? categoryId = null, bool? activeOnly = true)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .AsQueryable();

        if (activeOnly == true)
            query = query.Where(p => p.IsActive);

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId);

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(p => p.Name.Contains(searchTerm) || p.Barcode.Contains(searchTerm));

        return await query.Select(p => MapToServiceModel(p)).ToListAsync();
    }

    public async Task<ProductServiceModel?> GetByIdAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);

        return product == null ? null : MapToServiceModel(product);
    }

    public async Task<int> CreateAsync(ProductServiceModel model)
    {
        var product = new Product
        {
            Name = model.Name,
            Description = model.Description,
            Barcode = model.Barcode,
            Price = model.Price,
            CostPrice = model.CostPrice,
            StockQuantity = model.StockQuantity,
            MinimumStockLevel = model.MinimumStockLevel,
            Unit = model.Unit,
            ImageUrl = model.ImageUrl,
            IsActive = model.IsActive,
            CategoryId = model.CategoryId,
            SupplierId = model.SupplierId,
            CreatedOn = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product.Id;
    }

    public async Task<bool> UpdateAsync(ProductServiceModel model)
    {
        var product = await _context.Products.FindAsync(model.Id);
        if (product == null) return false;

        product.Name = model.Name;
        product.Description = model.Description;
        product.Barcode = model.Barcode;
        product.Price = model.Price;
        product.CostPrice = model.CostPrice;
        product.StockQuantity = model.StockQuantity;
        product.MinimumStockLevel = model.MinimumStockLevel;
        product.Unit = model.Unit;
        product.ImageUrl = model.ImageUrl;
        product.IsActive = model.IsActive;
        product.CategoryId = model.CategoryId;
        product.SupplierId = model.SupplierId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        product.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
        => await _context.Products.AnyAsync(p => p.Id == id);

    public async Task<IEnumerable<ProductServiceModel>> GetLowStockProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => p.IsActive && p.StockQuantity <= p.MinimumStockLevel)
            .Select(p => MapToServiceModel(p))
            .ToListAsync();
    }

    public async Task<bool> UpdateStockAsync(int productId, int quantity)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null) return false;

        product.StockQuantity += quantity;
        if (product.StockQuantity < 0) product.StockQuantity = 0;

        await _context.SaveChangesAsync();
        return true;
    }

    private static ProductServiceModel MapToServiceModel(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Barcode = p.Barcode,
        Price = p.Price,
        CostPrice = p.CostPrice,
        StockQuantity = p.StockQuantity,
        MinimumStockLevel = p.MinimumStockLevel,
        Unit = p.Unit,
        ImageUrl = p.ImageUrl,
        IsActive = p.IsActive,
        CategoryId = p.CategoryId,
        CategoryName = p.Category?.Name ?? string.Empty,
        SupplierId = p.SupplierId,
        SupplierName = p.Supplier?.Name
    };
}
