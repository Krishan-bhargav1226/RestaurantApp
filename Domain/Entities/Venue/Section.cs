using System.ComponentModel.DataAnnotations;
using Domain.Entities.Enums;

namespace Domain.Entities;

public class Section : TenantEntity
{
    public int OutletId { get; set; }

    [Required, MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public SectionType SectionType { get; set; }

    public int? FloorNumber { get; set; }

    public int SortOrder { get; set; }
}