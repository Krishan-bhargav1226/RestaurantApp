using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Ingredients
{
    public class CreateUpdateIngredientDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Unit { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal CostPerUnit { get; set; }
    }
}
