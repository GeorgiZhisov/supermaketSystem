using Microsoft.EntityFrameworkCore;
using SupermarketManagerSystem.Data;
using SupermarketManagerSystem.Data.Models;
using SupermarketManagerSystem.Services.Contracts;
using SupermarketManagerSystem.Services.Models;

namespace SupermarketManagerSystem.Services.Implementations;

public class SupplierService : ISupplierService
{
    private readonly ApplicationDbContext _context;

    public SupplierService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SupplierServiceModel>> GetAllAsync(bool? activeOnly = null)
    {
        var query = _context.Suppliers.AsQueryable();
        if (activeOnly.HasValue)
            query = query.Where(s => s.IsActive == activeOnly);

        return await query
            .Select(s => new SupplierServiceModel
            {
                Id = s.Id,
                Name = s.Name,
                ContactPerson = s.ContactPerson,
                Email = s.Email,
                Phone = s.Phone,
                Address = s.Address,
                IsActive = s.IsActive,
                ProductCount = s.Products.Count(p => p.IsActive)
            })
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<SupplierServiceModel?> GetByIdAsync(int id)
    {
        return await _context.Suppliers
            .Where(s => s.Id == id)
            .Select(s => new SupplierServiceModel
            {
                Id = s.Id,
                Name = s.Name,
                ContactPerson = s.ContactPerson,
                Email = s.Email,
                Phone = s.Phone,
                Address = s.Address,
                IsActive = s.IsActive,
                ProductCount = s.Products.Count(p => p.IsActive)
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateAsync(SupplierServiceModel model)
    {
        var supplier = new Supplier
        {
            Name = model.Name,
            ContactPerson = model.ContactPerson,
            Email = model.Email,
            Phone = model.Phone,
            Address = model.Address,
            IsActive = model.IsActive
        };
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        return supplier.Id;
    }

    public async Task<bool> UpdateAsync(SupplierServiceModel model)
    {
        var supplier = await _context.Suppliers.FindAsync(model.Id);
        if (supplier == null) return false;

        supplier.Name = model.Name;
        supplier.ContactPerson = model.ContactPerson;
        supplier.Email = model.Email;
        supplier.Phone = model.Phone;
        supplier.Address = model.Address;
        supplier.IsActive = model.IsActive;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null) return false;
        supplier.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
        => await _context.Suppliers.AnyAsync(s => s.Id == id);
}
