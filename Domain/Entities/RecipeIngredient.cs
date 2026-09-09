using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class RecipeIngredient : BaseEntity
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int IngredientId { get; set; }

        public decimal Quantity { get; set; }

        [Required]
        public string Unit { get; set; } = string.Empty;
    }
}
