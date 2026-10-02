using Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Subscription : TenantEntity
{
    public int PlanId { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Trial;
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime CurrentPeriodStart { get; set; } = DateTime.UtcNow;
    public DateTime CurrentPeriodEnd { get; set; } = DateTime.UtcNow.AddMonths(1);
    public DateTime? CancelledAt { get; set; }
    public bool AutoRenew { get; set; } = true;

    [MaxLength(150)]
    public string? ProviderCustomerId { get; set; }

    [MaxLength(150)]
    public string? ProviderSubscriptionId { get; set; }
}