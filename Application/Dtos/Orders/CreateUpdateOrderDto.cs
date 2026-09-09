using System.ComponentModel.DataAnnotations;
using Application.Dtos.OrderItems;
using Domain.Entities.Enums;

namespace Application.Dtos.Orders;

public class CreateUpdateOrderDto
{
    [Range(1, int.MaxValue)] public int BranchId { get; set; }
    public int? TableId { get; set; }
    [Required] public OrderType OrderType { get; set; }
    [MaxLength(20)] public string? TableNumber { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime? ScheduledAt { get; set; }
    [Range(0, double.MaxValue)] public decimal DiscountAmount { get; set; }
    [Range(0, double.MaxValue)] public decimal DeliveryCharge { get; set; }
    [MaxLength(1000)] public string? Notes { get; set; }
    public int? CreatedByUserId { get; set; }
    [Required, MinLength(1)] public List<CreateUpdateOrderItemDto> Items { get; set; } = new();
}
