using Domain.Entities.Enums;

namespace Application.Dtos.Products
{
    public class ProductResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal BasePrice { get; set; }

        public string? ImagePath { get; set; }

        public int CategoryId { get; set; }

        public FoodType FoodType { get; set; }

        public int PrepTimeMinutes { get; set; }

        public int? CalorieCount { get; set; }

        public bool IsAvailable { get; set; }
    }
}
