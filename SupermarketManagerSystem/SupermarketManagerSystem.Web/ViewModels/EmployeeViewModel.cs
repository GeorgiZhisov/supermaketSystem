using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SupermarketManagerSystem.Web.ViewModels;

public class EmployeeViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, MinimumLength = 2)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, MinimumLength = 2)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(100)]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Phone]
    [StringLength(20)]
    [Display(Name = "Phone")]
    public string? Phone { get; set; }

    [Required]
    [Display(Name = "Position")]
    public string Position { get; set; } = string.Empty;

    [Display(Name = "Hire Date")]
    [DataType(DataType.Date)]
    public DateTime HireDate { get; set; } = DateTime.Today;

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    [StringLength(500)]
    [Display(Name = "Notes")]
    public string? Notes { get; set; }

    public string FullName => $"{FirstName} {LastName}";
    public IEnumerable<SelectListItem> Positions { get; set; } = Enumerable.Empty<SelectListItem>();
}
