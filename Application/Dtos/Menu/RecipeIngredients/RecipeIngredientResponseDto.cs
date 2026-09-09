namespace Application.Dtos.RecipeIngredients
{
    public class RecipeIngredientResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int IngredientId { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
