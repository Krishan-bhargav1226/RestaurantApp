using System.ComponentModel.DataAnnotations;
using Domain.Entities.Enums;

namespace Domain.Entities;

public class Order : BaseEntity
{
    [Required]
    public int BranchId { get; set; }

    public int? CustomerId { get; set; }

    public int? TableId { get; set; }

    public int? AddressId { get; set; }

    [Required]
    public OrderType OrderType { get; set; }

    [MaxLength(20)]
    public string? TableNumber { get; set; }

    [Required]
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public DateTime? ScheduledAt { get; set; }

    public decimal SubTotal { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal DeliveryCharge { get; set; }

    public decimal GrandTotal { get; set; }

    public decimal TipAmount { get; set; }

    [MaxLength(3)]
    public string Currency { get; set; } = "INR";

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? VenueId { get; set; }

    public int? OutletId { get; set; }

    public int? SectionId { get; set; }

    public int? SeatTableId { get; set; }

    public int? QRCodeId { get; set; }

    [MaxLength(40)]
    public string? OrderNumber { get; set; }

    public ParosaOrderType? ParosaOrderType { get; set; }

    public ParosaOrderStatus WorkflowStatus { get; set; } = ParosaOrderStatus.Placed;

    [MaxLength(500)]
    public string? RejectionReason { get; set; }

    [MaxLength(500)]
    public string? CancellationReason { get; set; }

    public DateTime? PaymentPendingAt { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime? AcceptedAt { get; set; }

    public DateTime? RejectedAt { get; set; }

    public DateTime? PreparingAt { get; set; }

    public DateTime? PreparedAt { get; set; }

    public DateTime? PickedUpAt { get; set; }

    public DateTime? DeliveredAt { get; set; }

    public DateTime? ConfirmedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public DateTime? RefundedAt { get; set; }

    public int? ReceptionUserId { get; set; }

    public int? AssignedRunnerId { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    public ICollection<OrderStatusHistory> StatusHistory { get; set; } = new List<OrderStatusHistory>();
}