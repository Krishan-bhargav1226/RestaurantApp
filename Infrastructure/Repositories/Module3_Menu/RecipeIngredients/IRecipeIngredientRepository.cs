using Domain.Entities;

namespace Infrastructure.Repositories.RecipeIngredients
{
    public interface IRecipeIngredientRepository
    {
        Task<RecipeIngredient> CreateAsync(RecipeIngredient recipeIngredient);
        Task<List<RecipeIngredient>> GetAllAsync();
        Task<RecipeIngredient?> GetByIdAsync(int id);
        Task<RecipeIngredient> UpdateAsync(RecipeIngredient recipeIngredient);
        Task DeleteAsync(RecipeIngredient recipeIngredient);
    }
}
