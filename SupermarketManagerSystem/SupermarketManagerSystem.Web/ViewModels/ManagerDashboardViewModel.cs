using SupermarketManagerSystem.Web.ViewModels;

namespace SupermarketManagerSystem.Web.ViewModels;

public class ManagerDashboardViewModel
{
    public int TotalProducts { get; set; }
    public int TotalCategories { get; set; }
    public int TotalSuppliers { get; set; }
    public int TotalEmployees { get; set; }
    public int LowStockProductsCount { get; set; }
    public int PendingRestockRequests { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public List<ProductListViewModel> LowStockProducts { get; set; } = new();
    public List<RestockRequestViewModel> RecentRestockRequests { get; set; } = new();
}
