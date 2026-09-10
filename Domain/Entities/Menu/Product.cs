using Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Product : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal BasePrice { get; set; }

    public string? ImagePath { get; set; }

    public int CategoryId { get; set; }

    public FoodType FoodType { get; set; }

    public int PrepTimeMinutes { get; set; } = 15;

    public int? CalorieCount { get; set; }

    public bool IsAvailable { get; set; } = true;
}
