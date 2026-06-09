using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SupermarketManagerSystem.Data.Models;

public class ApplicationUser : IdentityUser
{
    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    public DateTime RegisteredOn { get; set; } = DateTime.UtcNow;

    public string FullName => $"{FirstName} {LastName}".Trim();
}
