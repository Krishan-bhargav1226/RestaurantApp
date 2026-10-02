using Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Refund : TenantEntity
{
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public RefundStatus Status { get; set; } = RefundStatus.Pending;

    [MaxLength(100)]
    public string? ProviderRefundId { get; set; }

    [MaxLength(500)]
    public string? Reason { get; set; }

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }

    [MaxLength(500)]
    public string? FailureReason { get; set; }
}

public class Coupon : TenantEntity
{
    [Required, MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    public CouponType Type { get; set; }
    public decimal Value { get; set; }
    public decimal MinimumOrderAmount { get; set; }
    public decimal? MaximumDiscountAmount { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime? EndsAt { get; set; }
    public int? UsageLimit { get; set; }
    public int UsageCount { get; set; }
    public int? PerCustomerLimit { get; set; }
}

public class CouponRedemption : TenantEntity
{
    public int CouponId { get; set; }
    public int CustomerId { get; set; }
    public int OrderId { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime RedeemedAt { get; set; } = DateTime.UtcNow;
}