using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Plan : BaseEntity
{
    [Required, MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required, MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public decimal MonthlyPrice { get; set; }
    public decimal AnnualPrice { get; set; }
    public int MaxOutlets { get; set; }
    public int MaxStaff { get; set; }
    public int MaxOrdersPerMonth { get; set; }
    public string? FeaturesJson { get; set; }
}