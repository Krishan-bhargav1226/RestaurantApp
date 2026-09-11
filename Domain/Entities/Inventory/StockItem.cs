using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class StockItem : BaseEntity
{
    [Required]
    public int BranchId { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Unit { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal ReorderLevel { get; set; }

    public bool IsActive { get; set; } = true;
}