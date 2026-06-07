using Microsoft.EntityFrameworkCore;
using SupermarketManagerSystem.Data;
using SupermarketManagerSystem.Data.Models;
using SupermarketManagerSystem.Services.Contracts;
using SupermarketManagerSystem.Services.Models;

namespace SupermarketManagerSystem.Services.Implementations;

public class RestockRequestService : IRestockRequestService
{
    private readonly ApplicationDbContext _context;

    public RestockRequestService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RestockRequestServiceModel>> GetAllAsync(string? statusFilter = null)
    {
        var query = _context.RestockRequests
            .Include(r => r.Product)
            .Include(r => r.Supplier)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(statusFilter) && Enum.TryParse<RestockRequestStatus>(statusFilter, out var status))
            query = query.Where(r => r.Status == status);

        return await query
            .OrderByDescending(r => r.RequestedOn)
            .Select(r => MapToServiceModel(r))
            .ToListAsync();
    }

    public async Task<RestockRequestServiceModel?> GetByIdAsync(int id)
    {
        var request = await _context.RestockRequests
            .Include(r => r.Product)
            .Include(r => r.Supplier)
            .FirstOrDefaultAsync(r => r.Id == id);

        return request == null ? null : MapToServiceModel(request);
    }

    public async Task<int> CreateAsync(RestockRequestServiceModel model)
    {
        var request = new RestockRequest
        {
            ProductId = model.ProductId,
            SupplierId = model.SupplierId,
            RequestedQuantity = model.RequestedQuantity,
            EstimatedCost = model.EstimatedCost,
            Notes = model.Notes,
            Status = RestockRequestStatus.Pending,
            RequestedOn = DateTime.UtcNow
        };
        _context.RestockRequests.Add(request);
        await _context.SaveChangesAsync();
        return request.Id;
    }

    public async Task<bool> ApproveAsync(int id)
    {
        var request = await _context.RestockRequests.FindAsync(id);
        if (request == null || request.Status != RestockRequestStatus.Pending) return false;

        request.Status = RestockRequestStatus.Approved;
        request.ApprovedOn = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelAsync(int id)
    {
        var request = await _context.RestockRequests.FindAsync(id);
        if (request == null) return false;

        request.Status = RestockRequestStatus.Cancelled;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MarkAsReceivedAsync(int id)
    {
        var request = await _context.RestockRequests.FindAsync(id);
        if (request == null) return false;

        request.Status = RestockRequestStatus.Received;
        request.ReceivedOn = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<RestockRequestServiceModel>> GetPendingAsync()
    {
        return await _context.RestockRequests
            .Include(r => r.Product)
            .Include(r => r.Supplier)
            .Where(r => r.Status == RestockRequestStatus.Pending)
            .Select(r => MapToServiceModel(r))
            .ToListAsync();
    }

    private static RestockRequestServiceModel MapToServiceModel(RestockRequest r) => new()
    {
        Id = r.Id,
        ProductId = r.ProductId,
        ProductName = r.Product?.Name ?? string.Empty,
        SupplierId = r.SupplierId,
        SupplierName = r.Supplier?.Name,
        RequestedQuantity = r.RequestedQuantity,
        EstimatedCost = r.EstimatedCost,
        Status = r.Status.ToString(),
        Notes = r.Notes,
        RequestedOn = r.RequestedOn,
        ApprovedOn = r.ApprovedOn,
        ReceivedOn = r.ReceivedOn
    };
}
