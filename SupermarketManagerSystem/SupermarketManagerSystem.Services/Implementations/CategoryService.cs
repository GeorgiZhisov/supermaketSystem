using Microsoft.EntityFrameworkCore;
using SupermarketManagerSystem.Data;
using SupermarketManagerSystem.Data.Models;
using SupermarketManagerSystem.Services.Contracts;
using SupermarketManagerSystem.Services.Models;

namespace SupermarketManagerSystem.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryServiceModel>> GetAllAsync()
    {
        return await _context.Categories
            .Select(c => new CategoryServiceModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ProductCount = c.Products.Count(p => p.IsActive)
            })
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<CategoryServiceModel?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .Where(c => c.Id == id)
            .Select(c => new CategoryServiceModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ProductCount = c.Products.Count(p => p.IsActive)
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateAsync(CategoryServiceModel model)
    {
        var category = new Category
        {
            Name = model.Name,
            Description = model.Description
        };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category.Id;
    }

    public async Task<bool> UpdateAsync(CategoryServiceModel model)
    {
        var category = await _context.Categories.FindAsync(model.Id);
        if (category == null) return false;

        category.Name = model.Name;
        category.Description = model.Description;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
        => await _context.Categories.AnyAsync(c => c.Id == id);
}
