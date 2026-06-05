using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SupermarketManagerSystem.Web.ViewModels;

public class StockMovementViewModel
{
    [Required]
    [Display(Name = "Product")]
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Transaction Type")]
    public string Type { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    [Display(Name = "Quantity")]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue)]
    [Display(Name = "Unit Price (BGN)")]
    public decimal UnitPrice { get; set; }

    [StringLength(500)]
    [Display(Name = "Notes")]
    public string? Notes { get; set; }

    public IEnumerable<SelectListItem> Products { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> TransactionTypes { get; set; } = Enumerable.Empty<SelectListItem>();
}
