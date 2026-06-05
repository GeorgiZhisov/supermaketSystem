namespace SupermarketManagerSystem.Web.ViewModels;

public class ProductListViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int MinimumStockLevel { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? SupplierName { get; set; }
    public bool IsActive { get; set; }
    public bool IsLowStock => StockQuantity <= MinimumStockLevel;
}
