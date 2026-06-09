using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermarketManagerSystem.Data.Models;

public enum RestockRequestStatus
{
    Pending = 0,
    Approved = 1,
    Ordered = 2,
    Received = 3,
    Cancelled = 4
}

public class RestockRequest
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public int RequestedQuantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? EstimatedCost { get; set; }

    public RestockRequestStatus Status { get; set; } = RestockRequestStatus.Pending;

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public DateTime RequestedOn { get; set; } = DateTime.UtcNow;

    public DateTime? ApprovedOn { get; set; }

    public DateTime? ReceivedOn { get; set; }

    public string? RequestedByUserId { get; set; }
    public ApplicationUser? RequestedByUser { get; set; }
}
