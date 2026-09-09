using Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Products
{
    public class CreateUpdateProductDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal BasePrice { get; set; }

        public string? ImagePath { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public FoodType FoodType { get; set; }

        [Range(1, 300)]
        public int PrepTimeMinutes { get; set; } = 15;

        [Range(0, 5000)]
        public int? CalorieCount { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}
