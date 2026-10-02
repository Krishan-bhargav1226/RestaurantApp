using Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class OrderStatusHistory : TenantEntity
{
    public int OrderId { get; set; }
    public ParosaOrderStatus? FromStatus { get; set; }
    public ParosaOrderStatus ToStatus { get; set; }
    public int? ChangedByUserId { get; set; }
    public OrderStatusChangeSource Source { get; set; } = OrderStatusChangeSource.System;

    [MaxLength(500)]
    public string? Reason { get; set; }

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}

public class OrderItemAddOn : BaseEntity
{
    public int OrderItemId { get; set; }
    public int? AddOnId { get; set; }

    [Required, MaxLength(120)]
    public string AddOnNameSnapshot { get; set; } = string.Empty;

    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
}

public class Cart : TenantEntity
{
    public int OutletId { get; set; }
    public int? SeatTableId { get; set; }
    public int? CustomerId { get; set; }

    [Required, MaxLength(200)]
    public string SessionToken { get; set; } = string.Empty;

    public CartStatus Status { get; set; } = CartStatus.Active;
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddHours(2);
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal GrandTotal { get; set; }

    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}

public class CartItem : TenantEntity
{
    public int CartId { get; set; }
    public int MenuItemId { get; set; }
    public int? VariantId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    [MaxLength(500)]
    public string? SpecialInstructions { get; set; }

    public ICollection<CartItemAddOn> AddOns { get; set; } = new List<CartItemAddOn>();
}

public class CartItemAddOn : TenantEntity
{
    public int CartItemId { get; set; }
    public int AddOnId { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
}