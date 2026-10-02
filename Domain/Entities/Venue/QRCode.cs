using System.ComponentModel.DataAnnotations;
using Domain.Entities.Enums;

namespace Domain.Entities;

public class QRCode : TenantEntity
{
    public int OutletId { get; set; }

    public int? SeatTableId { get; set; }

    [Required]
    public QRCodeType QRCodeType { get; set; }

    [Required, MaxLength(100)]
    public string Code { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Token { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? TargetUrl { get; set; }

    public QRCodeStatus Status { get; set; } = QRCodeStatus.Active;

    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

    public DateTime? RevokedAt { get; set; }
}