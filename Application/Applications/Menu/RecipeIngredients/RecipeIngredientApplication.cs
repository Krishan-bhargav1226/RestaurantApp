using Application.Dtos.RecipeIngredients;
using AutoMapper;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Repositories.RecipeIngredients;
using Microsoft.EntityFrameworkCore;

namespace Application.Applications.RecipeIngredients
{
    public class RecipeIngredientApplication : IRecipeIngredientApplication
    {
        private readonly IRecipeIngredientRepository _recipeIngredientRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RecipeIngredientApplication(
            IRecipeIngredientRepository recipeIngredientRepository,
            DataContext context,
            IMapper mapper)
        {
            _recipeIngredientRepository = recipeIngredientRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<RecipeIngredientResponseDto> CreateAsync(CreateUpdateRecipeIngredientDto input)
        {
            await EnsureReferencesAsync(input.ProductId, input.IngredientId);

            var duplicate = await _context.RecipeIngredients.AnyAsync(x =>
                x.ProductId == input.ProductId &&
                x.IngredientId == input.IngredientId);

            if (duplicate)
                throw new InvalidOperationException("This ingredient is already assigned to the product.");

            var recipeIngredient = _mapper.Map<RecipeIngredient>(input);
            var createdRecipeIngredient = await _recipeIngredientRepository.CreateAsync(recipeIngredient);
            return _mapper.Map<RecipeIngredientResponseDto>(createdRecipeIngredient);
        }

        public async Task<List<RecipeIngredientResponseDto>> GetAllAsync()
        {
            var recipeIngredients = await _recipeIngredientRepository.GetAllAsync();
            return _mapper.Map<List<RecipeIngredientResponseDto>>(recipeIngredients);
        }

        public async Task<RecipeIngredientResponseDto> GetByIdAsync(int id)
        {
            var recipeIngredient = await _recipeIngredientRepository.GetByIdAsync(id);

            if (recipeIngredient == null)
                throw new KeyNotFoundException("RecipeIngredient not found.");

            return _mapper.Map<RecipeIngredientResponseDto>(recipeIngredient);
        }

        public async Task<RecipeIngredientResponseDto> UpdateAsync(int id, CreateUpdateRecipeIngredientDto input)
        {
            var recipeIngredient = await _recipeIngredientRepository.GetByIdAsync(id);

            if (recipeIngredient == null)
                throw new KeyNotFoundException("RecipeIngredient not found.");

            await EnsureReferencesAsync(input.ProductId, input.IngredientId);

            var duplicate = await _context.RecipeIngredients.AnyAsync(x =>
                x.Id != id &&
                x.ProductId == input.ProductId &&
                x.IngredientId == input.IngredientId);

            if (duplicate)
                throw new InvalidOperationException("This ingredient is already assigned to the product.");

            _mapper.Map(input, recipeIngredient);
            var updatedRecipeIngredient = await _recipeIngredientRepository.UpdateAsync(recipeIngredient);
            return _mapper.Map<RecipeIngredientResponseDto>(updatedRecipeIngredient);
        }

        public async Task DeleteAsync(int id)
        {
            var recipeIngredient = await _recipeIngredientRepository.GetByIdAsync(id);

            if (recipeIngredient == null)
                throw new KeyNotFoundException("RecipeIngredient not found.");

            await _recipeIngredientRepository.DeleteAsync(recipeIngredient);
        }

        private async Task EnsureReferencesAsync(int productId, int ingredientId)
        {
            if (!await _context.Products.AnyAsync(x => x.Id == productId))
                throw new KeyNotFoundException("Product not found.");

            if (!await _context.Ingredients.AnyAsync(x => x.Id == ingredientId))
                throw new KeyNotFoundException("Ingredient not found.");
        }
    }
}
