using System.ComponentModel.DataAnnotations;

namespace SupermarketManagerSystem.Web.ViewModels;

public class SupplierViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Supplier name is required.")]
    [StringLength(200, MinimumLength = 2)]
    [Display(Name = "Company Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200)]
    [Display(Name = "Contact Person")]
    public string? ContactPerson { get; set; }

    [EmailAddress]
    [StringLength(100)]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Phone]
    [StringLength(20)]
    [Display(Name = "Phone")]
    public string? Phone { get; set; }

    [StringLength(500)]
    [Display(Name = "Address")]
    public string? Address { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    public int ProductCount { get; set; }
}
