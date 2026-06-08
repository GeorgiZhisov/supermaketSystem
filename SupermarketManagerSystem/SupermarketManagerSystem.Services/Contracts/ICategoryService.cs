using SupermarketManagerSystem.Services.Models;

namespace SupermarketManagerSystem.Services.Contracts;

public interface ICategoryService
{
    Task<IEnumerable<CategoryServiceModel>> GetAllAsync();
    Task<CategoryServiceModel?> GetByIdAsync(int id);
    Task<int> CreateAsync(CategoryServiceModel model);
    Task<bool> UpdateAsync(CategoryServiceModel model);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
