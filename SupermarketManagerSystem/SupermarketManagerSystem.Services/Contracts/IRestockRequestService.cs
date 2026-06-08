using SupermarketManagerSystem.Services.Models;

namespace SupermarketManagerSystem.Services.Contracts;

public interface IRestockRequestService
{
    Task<IEnumerable<RestockRequestServiceModel>> GetAllAsync(string? statusFilter = null);
    Task<RestockRequestServiceModel?> GetByIdAsync(int id);
    Task<int> CreateAsync(RestockRequestServiceModel model);
    Task<bool> ApproveAsync(int id);
    Task<bool> CancelAsync(int id);
    Task<bool> MarkAsReceivedAsync(int id);
    Task<IEnumerable<RestockRequestServiceModel>> GetPendingAsync();
}
