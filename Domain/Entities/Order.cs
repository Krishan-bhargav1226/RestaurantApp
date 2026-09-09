using System.ComponentModel.DataAnnotations;
using Domain.Entities.Enums;

namespace Domain.Entities;

public class Order : BaseEntity
{
    [Required] public int BranchId { get; set; }
    public int? CustomerId { get; set; }
    public int? TableId { get; set; }
    public int? AddressId { get; set; }
    [Required] public OrderType OrderType { get; set; }
    [MaxLength(20)] public string? TableNumber { get; set; }
    [Required] public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? ScheduledAt { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DeliveryCharge { get; set; }
    public decimal GrandTotal { get; set; }
    [MaxLength(1000)] public string? Notes { get; set; }
    public int? CreatedByUserId { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
