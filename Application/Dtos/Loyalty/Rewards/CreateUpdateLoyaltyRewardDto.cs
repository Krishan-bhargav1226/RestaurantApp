using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.LoyaltyRewards;

public class CreateUpdateLoyaltyRewardDto
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