namespace SupermarketManagerSystem.Services.Models;

public class RestockRequestServiceModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int? SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public int RequestedQuantity { get; set; }
    public decimal? EstimatedCost { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime RequestedOn { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public DateTime? ReceivedOn { get; set; }
}
