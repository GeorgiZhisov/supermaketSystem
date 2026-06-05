using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SupermarketManagerSystem.Web.ViewModels;

public class ProductFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 200 characters.")]
    [Display(Name = "Product Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Barcode is required.")]
    [StringLength(50)]
    [Display(Name = "Barcode")]
    public string Barcode { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999,999.99.")]
    [Display(Name = "Selling Price (BGN)")]
    public decimal Price { get; set; }

    [Range(0, 999999.99)]
    [Display(Name = "Cost Price (BGN)")]
    public decimal CostPrice { get; set; }

    [Range(0, int.MaxValue)]
    [Display(Name = "Stock Quantity")]
    public int StockQuantity { get; set; }

    [Range(0, int.MaxValue)]
    [Display(Name = "Minimum Stock Level")]
    public int MinimumStockLevel { get; set; } = 10;

    [StringLength(50)]
    [Display(Name = "Unit (e.g. kg, pcs, l)")]
    public string? Unit { get; set; }

    [StringLength(500)]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "Please select a category.")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Display(Name = "Supplier")]
    public int? SupplierId { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> Suppliers { get; set; } = Enumerable.Empty<SelectListItem>();
}
