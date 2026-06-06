using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketManagerSystem.Data.Models;

namespace SupermarketManagerSystem.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrator")]
public class UsersController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        var vm = new List<UserManagementViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            vm.Add(new UserManagementViewModel
            {
                Id = user.Id,
                Email = user.Email ?? "",
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
                IsLocked = await _userManager.IsLockedOutAsync(user),
                Roles = roles.ToList(),
                RegisteredOn = user.RegisteredOn
            });
        }

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Ban(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        // Cannot ban yourself
        if (user.Email == User.Identity!.Name)
        {
            TempData["Error"] = "You cannot ban your own account.";
            return RedirectToAction(nameof(Index));
        }

        await _userManager.SetLockoutEnabledAsync(user, true);
        await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
        TempData["Success"] = $"User {user.Email} has been banned.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Unban(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        await _userManager.SetLockoutEndDateAsync(user, null);
        TempData["Success"] = $"User {user.Email} has been unbanned.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> MakeAdmin(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        if (!await _userManager.IsInRoleAsync(user, "Administrator"))
            await _userManager.AddToRoleAsync(user, "Administrator");

        TempData["Success"] = $"User {user.Email} is now an Administrator.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveAdmin(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        if (user.Email == User.Identity!.Name)
        {
            TempData["Error"] = "You cannot remove your own admin role.";
            return RedirectToAction(nameof(Index));
        }

        await _userManager.RemoveFromRoleAsync(user, "Administrator");
        TempData["Success"] = $"Admin role removed from {user.Email}.";
        return RedirectToAction(nameof(Index));
    }
}

public class UserManagementViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsLocked { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime RegisteredOn { get; set; }
}
