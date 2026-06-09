using System.ComponentModel.DataAnnotations;

namespace SupermarketManagerSystem.Data.Models;

public enum EmployeePosition
{
    Cashier = 0,
    StockClerk = 1,
    Supervisor = 2,
    Manager = 3,
    SecurityGuard = 4
}

public class Employee
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(100)]
    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    public EmployeePosition Position { get; set; }

    public DateTime HireDate { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    [MaxLength(500)]
    public string? Notes { get; set; }

    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
