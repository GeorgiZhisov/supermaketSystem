using SupermarketManagerSystem.Services.Models;

namespace SupermarketManagerSystem.Services.Contracts;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeServiceModel>> GetAllAsync(bool? activeOnly = null);
    Task<EmployeeServiceModel?> GetByIdAsync(int id);
    Task<int> CreateAsync(EmployeeServiceModel model);
    Task<bool> UpdateAsync(EmployeeServiceModel model);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
