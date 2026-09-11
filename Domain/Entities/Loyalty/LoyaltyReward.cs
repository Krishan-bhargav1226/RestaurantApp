using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class LoyaltyReward : BaseEntity
{
    [Required]
    public int BranchId { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Range(1, int.MaxValue)]
    public int PointsRequired { get; set; }

    public bool IsActive { get; set; } = true;
}