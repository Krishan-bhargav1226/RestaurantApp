using Application.Dtos.OrderItems;
using Domain.Entities.Enums;

namespace Application.Dtos.Orders;

public class OrderResponseDto
{
    public int Id { get; set; }
    public int BranchId { get; set; }
    public int? CustomerId { get; set; }
    public int? TableId { get; set; }
    public int? AddressId { get; set; }
    public OrderType OrderType { get; set; }
    public string? TableNumber { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DeliveryCharge { get; set; }
    public decimal GrandTotal { get; set; }
    public string? Notes { get; set; }
    public int? CreatedByUserId { get; set; }
    public List<OrderItemResponseDto> Items { get; set; } = new();
}
