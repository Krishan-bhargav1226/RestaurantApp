using Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Notification : TenantEntity
{
    public int? UserId { get; set; }
    public int? CustomerId { get; set; }
    public NotificationType Type { get; set; }
    public NotificationChannel Channel { get; set; } = NotificationChannel.InApp;

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(1000)]
    public string Message { get; set; } = string.Empty;

    public string? DataJson { get; set; }
    public bool IsRead { get; set; }
    public DateTime? SentAt { get; set; }
}

public class StaffShift : TenantEntity
{
    public int OutletId { get; set; }
    public int UserId { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public StaffShiftStatus Status { get; set; } = StaffShiftStatus.Scheduled;

    [MaxLength(500)]
    public string? Notes { get; set; }
}

public class AuditLog : TenantEntity
{
    public int? UserId { get; set; }

    [Required, MaxLength(100)]
    public string EntityName { get; set; } = string.Empty;

    public int? EntityId { get; set; }

    [Required, MaxLength(50)]
    public string Action { get; set; } = string.Empty;

    public string? OldValuesJson { get; set; }
    public string? NewValuesJson { get; set; }

    [MaxLength(64)]
    public string? IpAddress { get; set; }

    [MaxLength(500)]
    public string? UserAgent { get; set; }
}