using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermarketManagerSystem.Services.Contracts;
using SupermarketManagerSystem.Web.ViewModels;

namespace SupermarketManagerSystem.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrator")]
public class DashboardController : Controller
{
    private readonly IAdminService _adminService;

    public DashboardController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public async Task<IActionResult> Index()
    {
        var data = await _adminService.GetDashboardDataAsync();
        var vm = new ManagerDashboardViewModel
        {
            TotalProducts = data.TotalProducts,
            TotalCategories = data.TotalCategories,
            TotalSuppliers = data.TotalSuppliers,
            TotalEmployees = data.TotalEmployees,
            LowStockProductsCount = data.LowStockProductsCount,
            PendingRestockRequests = data.PendingRestockRequests,
            TotalInventoryValue = data.TotalInventoryValue,
            LowStockProducts = data.LowStockProducts.Select(p => new ProductListViewModel
            {
                Id = p.Id, Name = p.Name, StockQuantity = p.StockQuantity,
                MinimumStockLevel = p.MinimumStockLevel, CategoryName = p.CategoryName
            }).ToList(),
            RecentRestockRequests = data.RecentRestockRequests.Select(r => new RestockRequestViewModel
            {
                Id = r.Id, ProductName = r.ProductName, Status = r.Status,
                RequestedQuantity = r.RequestedQuantity, RequestedOn = r.RequestedOn
            }).ToList()
        };
        return View(vm);
    }
}
