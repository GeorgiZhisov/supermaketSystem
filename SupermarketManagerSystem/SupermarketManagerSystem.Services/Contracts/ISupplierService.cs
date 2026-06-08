using SupermarketManagerSystem.Services.Models;

namespace SupermarketManagerSystem.Services.Contracts;

public interface ISupplierService
{
    Task<IEnumerable<SupplierServiceModel>> GetAllAsync(bool? activeOnly = null);
    Task<SupplierServiceModel?> GetByIdAsync(int id);
    Task<int> CreateAsync(SupplierServiceModel model);
    Task<bool> UpdateAsync(SupplierServiceModel model);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
