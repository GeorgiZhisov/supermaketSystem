using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SupermarketManagerSystem.Web.ViewModels;

public class RestockRequestViewModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Product")]
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;

    [Display(Name = "Supplier")]
    public int? SupplierId { get; set; }
    public string? SupplierName { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    [Display(Name = "Requested Quantity")]
    public int RequestedQuantity { get; set; }

    [Range(0, double.MaxValue)]
    [Display(Name = "Estimated Cost (BGN)")]
    public decimal? EstimatedCost { get; set; }

    [Display(Name = "Status")]
    public string Status { get; set; } = "Pending";

    [StringLength(1000)]
    [Display(Name = "Notes")]
    public string? Notes { get; set; }

    public DateTime RequestedOn { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public DateTime? ReceivedOn { get; set; }

    public IEnumerable<SelectListItem> Products { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> Suppliers { get; set; } = Enumerable.Empty<SelectListItem>();
}
