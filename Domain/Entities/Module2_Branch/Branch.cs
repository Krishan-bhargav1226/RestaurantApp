using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Branch : BaseEntity
{
    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(300)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string State { get; set; } = string.Empty;

    // Postal codes are not arithmetic values: they can carry leading zeros and
    // must never overflow, so they are stored as text rather than int.
    [Required]
    [MaxLength(10)]
    public string PinCode { get; set; } = string.Empty;

    // Phone numbers can exceed int range and may contain '+', spaces or dashes.
    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(150)]
    [EmailAddress]
    public string? Email { get; set; }

    public TimeSpan OpeningTime { get; set; }

    public TimeSpan ClosingTime { get; set; }

    public bool IsOpen { get; set; }

    public bool AcceptsDineIn { get; set; }

    public bool AcceptsDelivery { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? DeliveryRadiusKm { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal? MinDeliveryOrderAmount { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? DeliveryCharge { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? TaxPercentage { get; set; }

    [Column(TypeName = "decimal(9,6)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(9,6)")]
    public decimal? Longitude { get; set; }
}
