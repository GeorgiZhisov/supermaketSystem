using SupermarketManagerSystem.Services.Models;

namespace SupermarketManagerSystem.Services.Contracts;

public interface IProductService
{
    Task<IEnumerable<ProductServiceModel>> GetAllAsync(string? searchTerm = null, int? categoryId = null, bool? activeOnly = true);
    Task<ProductServiceModel?> GetByIdAsync(int id);
    Task<int> CreateAsync(ProductServiceModel model);
    Task<bool> UpdateAsync(ProductServiceModel model);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<ProductServiceModel>> GetLowStockProductsAsync();
    Task<bool> UpdateStockAsync(int productId, int quantity);
}
