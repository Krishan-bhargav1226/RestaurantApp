using System.ComponentModel.DataAnnotations;
using Domain.Entities.Enums;

namespace Domain.Entities;

public class Venue : BaseEntity
{
    [Required, MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(160)]
    public string Slug { get; set; } = string.Empty;

    [Required]
    public VenueType VenueType { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public string? LogoUrl { get; set; }

    [MaxLength(30)]
    public string? Phone { get; set; }

    [MaxLength(150)]
    public string? Email { get; set; }

    [MaxLength(250)]
    public string? AddressLine1 { get; set; }

    [MaxLength(250)]
    public string? AddressLine2 { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(100)]
    public string? State { get; set; }

    [MaxLength(20)]
    public string? PostalCode { get; set; }

    [MaxLength(100)]
    public string Country { get; set; } = "India";

    [MaxLength(20)]
    public string Currency { get; set; } = "INR";

    [MaxLength(100)]
    public string? GSTIN { get; set; }

    public decimal DefaultTaxPercentage { get; set; }

    [MaxLength(100)]
    public string TimeZoneId { get; set; } = "Asia/Kolkata";

    public string? OperatingHoursJson { get; set; }

    public bool IsSuspended { get; set; }
}