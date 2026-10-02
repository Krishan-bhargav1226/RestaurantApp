using System.ComponentModel.DataAnnotations;
using Domain.Entities.Enums;

namespace Domain.Entities;

public class Outlet : TenantEntity
{
    [Required, MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? DisplayName { get; set; }

    [Required]
    public OutletType OutletType { get; set; }

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

    [MaxLength(30)]
    public string? Phone { get; set; }

    public TimeOnly? OpensAt { get; set; }

    public TimeOnly? ClosesAt { get; set; }

    public bool SupportsSeatDelivery { get; set; } = true;

    public bool SupportsPickup { get; set; } = true;

    public int SortOrder { get; set; }
}