using Application.Dtos.RecipeIngredients;

namespace Application.Applications.RecipeIngredients
{
    public interface IRecipeIngredientApplication
    {
        Task<RecipeIngredientResponseDto> CreateAsync(CreateUpdateRecipeIngredientDto input);
        Task<List<RecipeIngredientResponseDto>> GetAllAsync();
        Task<RecipeIngredientResponseDto> GetByIdAsync(int id);
        Task<RecipeIngredientResponseDto> UpdateAsync(int id, CreateUpdateRecipeIngredientDto input);
        Task DeleteAsync(int id);
    }
}
