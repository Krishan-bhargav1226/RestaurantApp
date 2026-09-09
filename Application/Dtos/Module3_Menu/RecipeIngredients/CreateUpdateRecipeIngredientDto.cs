using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.RecipeIngredients
{
    public class CreateUpdateRecipeIngredientDto
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int IngredientId { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Required]
        [MaxLength(50)]
        public string Unit { get; set; } = string.Empty;
    }
}
