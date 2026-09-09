using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Ingredient : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Unit { get; set; } = string.Empty;

    public decimal CostPerUnit { get; set; }
}
