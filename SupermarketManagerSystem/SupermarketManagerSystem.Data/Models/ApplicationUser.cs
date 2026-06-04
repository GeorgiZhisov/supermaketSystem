using Microsoft.AspNetCore.Identity;

namespace SupermarketManagerSystem.Data.Models;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
}
