using Microsoft.EntityFrameworkCore;
using SupermarketManagerSystem.Data;
using SupermarketManagerSystem.Data.Models;
using SupermarketManagerSystem.Services.Contracts;
using SupermarketManagerSystem.Services.Models;

namespace SupermarketManagerSystem.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext _context;

    public EmployeeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EmployeeServiceModel>> GetAllAsync(bool? activeOnly = null)
    {
        var query = _context.Employees.AsQueryable();
        if (activeOnly.HasValue)
            query = query.Where(e => e.IsActive == activeOnly);

        return await query
            .Select(e => new EmployeeServiceModel
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                Position = e.Position.ToString(),
                HireDate = e.HireDate,
                IsActive = e.IsActive,
                Notes = e.Notes
            })
            .OrderBy(e => e.LastName)
            .ToListAsync();
    }

    public async Task<EmployeeServiceModel?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .Where(e => e.Id == id)
            .Select(e => new EmployeeServiceModel
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                Position = e.Position.ToString(),
                HireDate = e.HireDate,
                IsActive = e.IsActive,
                Notes = e.Notes
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateAsync(EmployeeServiceModel model)
    {
        var employee = new Employee
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Phone = model.Phone,
            Position = Enum.Parse<EmployeePosition>(model.Position),
            HireDate = model.HireDate,
            IsActive = model.IsActive,
            Notes = model.Notes
        };
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee.Id;
    }

    public async Task<bool> UpdateAsync(EmployeeServiceModel model)
    {
        var employee = await _context.Employees.FindAsync(model.Id);
        if (employee == null) return false;

        employee.FirstName = model.FirstName;
        employee.LastName = model.LastName;
        employee.Email = model.Email;
        employee.Phone = model.Phone;
        employee.Position = Enum.Parse<EmployeePosition>(model.Position);
        employee.IsActive = model.IsActive;
        employee.Notes = model.Notes;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return false;
        employee.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
        => await _context.Employees.AnyAsync(e => e.Id == id);
}
