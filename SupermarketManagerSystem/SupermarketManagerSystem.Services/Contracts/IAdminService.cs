using SupermarketManagerSystem.Services.Models;

namespace SupermarketManagerSystem.Services.Contracts;

public interface IAdminService
{
    Task<DashboardServiceModel> GetDashboardDataAsync();
    Task<IEnumerable<string>> GetAllUserEmailsAsync();
    Task<bool> AssignRoleAsync(string userId, string role);
    Task<bool> RemoveRoleAsync(string userId, string role);
}
