using Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class SeatTable : TenantEntity
{
    public int OutletId { get; set; }

    public int SectionId { get; set; }

    [Required, MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Label { get; set; } = string.Empty;

    [Required]
    public LocationUnitType UnitType { get; set; }

    [MaxLength(20)]
    public string? RowLabel { get; set; }

    public int? Number { get; set; }

    public int Capacity { get; set; } = 1;

    public decimal? PositionX { get; set; }

    public decimal? PositionY { get; set; }

    public bool IsAvailable { get; set; } = true;

    public bool IsOccupied { get; set; }
}