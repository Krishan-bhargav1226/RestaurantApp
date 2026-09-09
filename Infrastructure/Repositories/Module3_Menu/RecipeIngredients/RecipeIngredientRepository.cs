using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.RecipeIngredients
{
    public class RecipeIngredientRepository : IRecipeIngredientRepository
    {
        private readonly DataContext _context;

        public RecipeIngredientRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<RecipeIngredient> CreateAsync(RecipeIngredient recipeIngredient)
        {
            _context.RecipeIngredients.Add(recipeIngredient);
            await _context.SaveChangesAsync();
            return recipeIngredient;
        }

        public async Task<List<RecipeIngredient>> GetAllAsync()
        {
            return await _context.RecipeIngredients
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<RecipeIngredient?> GetByIdAsync(int id)
        {
            return await _context.RecipeIngredients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<RecipeIngredient> UpdateAsync(RecipeIngredient recipeIngredient)
        {
            _context.RecipeIngredients.Update(recipeIngredient);
            await _context.SaveChangesAsync();
            return recipeIngredient;
        }

        public async Task DeleteAsync(RecipeIngredient recipeIngredient)
        {
            recipeIngredient.IsDeleted = true;
            _context.RecipeIngredients.Update(recipeIngredient);
            await _context.SaveChangesAsync();
        }
    }
}
