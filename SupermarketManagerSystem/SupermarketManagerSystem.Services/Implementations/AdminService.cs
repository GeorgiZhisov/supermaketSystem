using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SupermarketManagerSystem.Data;
using SupermarketManagerSystem.Data.Models;
using SupermarketManagerSystem.Services.Contracts;
using SupermarketManagerSystem.Services.Models;

namespace SupermarketManagerSystem.Services.Implementations;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<DashboardServiceModel> GetDashboardDataAsync()
    {
        var totalProducts = await _context.Products.CountAsync(p => p.IsActive);
        var totalCategories = await _context.Categories.CountAsync();
        var totalSuppliers = await _context.Suppliers.CountAsync(s => s.IsActive);
        var totalEmployees = await _context.Employees.CountAsync(e => e.IsActive);
        var lowStockProducts = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => p.IsActive && p.StockQuantity <= p.MinimumStockLevel)
            .ToListAsync();
        var pendingRequests = await _context.RestockRequests
            .CountAsync(r => r.Status == RestockRequestStatus.Pending);
        var inventoryValue = await _context.Products
            .Where(p => p.IsActive)
            .SumAsync(p => (decimal?)p.StockQuantity * p.CostPrice) ?? 0;
        var recentRequests = await _context.RestockRequests
            .Include(r => r.Product)
            .Include(r => r.Supplier)
            .OrderByDescending(r => r.RequestedOn)
            .Take(5)
            .ToListAsync();

        return new DashboardServiceModel
        {
            TotalProducts = totalProducts,
            TotalCategories = totalCategories,
            TotalSuppliers = totalSuppliers,
            TotalEmployees = totalEmployees,
            LowStockProductsCount = lowStockProducts.Count,
            PendingRestockRequests = pendingRequests,
            TotalInventoryValue = inventoryValue,
            LowStockProducts = lowStockProducts.Select(p => new ProductServiceModel
            {
                Id = p.Id, Name = p.Name, StockQuantity = p.StockQuantity,
                MinimumStockLevel = p.MinimumStockLevel, CategoryName = p.Category?.Name ?? ""
            }).ToList(),
            RecentRestockRequests = recentRequests.Select(r => new RestockRequestServiceModel
            {
                Id = r.Id, ProductName = r.Product?.Name ?? "", Status = r.Status.ToString(),
                RequestedQuantity = r.RequestedQuantity, RequestedOn = r.RequestedOn
            }).ToList()
        };
    }

    public async Task<IEnumerable<string>> GetAllUserEmailsAsync()
    {
        return await _context.Users.Select(u => u.Email ?? "").ToListAsync();
    }

    public async Task<bool> AssignRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;
        var result = await _userManager.AddToRoleAsync(user, role);
        return result.Succeeded;
    }

    public async Task<bool> RemoveRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;
        var result = await _userManager.RemoveFromRoleAsync(user, role);
        return result.Succeeded;
    }
}
