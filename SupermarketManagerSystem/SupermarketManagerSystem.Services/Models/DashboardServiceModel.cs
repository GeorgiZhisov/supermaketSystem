namespace SupermarketManagerSystem.Services.Models;

public class DashboardServiceModel
{
    public int TotalProducts { get; set; }
    public int TotalCategories { get; set; }
    public int TotalSuppliers { get; set; }
    public int TotalEmployees { get; set; }
    public int LowStockProductsCount { get; set; }
    public int PendingRestockRequests { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public List<ProductServiceModel> LowStockProducts { get; set; } = new();
    public List<RestockRequestServiceModel> RecentRestockRequests { get; set; } = new();
}
