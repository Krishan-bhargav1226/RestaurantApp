using Application.Dtos.Ingredients;

namespace Application.Applications.Ingredients
{
    public interface IIngredientApplication
    {
        Task<IngredientResponseDto> CreateAsync(CreateUpdateIngredientDto input);
        Task<List<IngredientResponseDto>> GetAllAsync();
        Task<IngredientResponseDto> GetByIdAsync(int id);
        Task<IngredientResponseDto> UpdateAsync(int id, CreateUpdateIngredientDto input);
        Task DeleteAsync(int id);
    }
}
