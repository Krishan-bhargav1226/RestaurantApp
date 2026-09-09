using Domain.Entities;

namespace Infrastructure.Repositories.Ingredients
{
    public interface IIngredientRepository
    {
        Task<Ingredient> CreateAsync(Ingredient ingredient);
        Task<List<Ingredient>> GetAllAsync();
        Task<Ingredient?> GetByIdAsync(int id);
        Task<Ingredient> UpdateAsync(Ingredient ingredient);
        Task DeleteAsync(Ingredient ingredient);
    }
}
