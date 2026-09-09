using Application.Dtos.RecipeIngredients;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.RecipeIngredients;

namespace Application.Applications.RecipeIngredients
{
    public class RecipeIngredientApplication : IRecipeIngredientApplication
    {
        private readonly IRecipeIngredientRepository _recipeIngredientRepository;
        private readonly IMapper _mapper;

        public RecipeIngredientApplication(
            IRecipeIngredientRepository recipeIngredientRepository,
            IMapper mapper)
        {
            _recipeIngredientRepository = recipeIngredientRepository;
            _mapper = mapper;
        }

        public async Task<RecipeIngredientResponseDto> CreateAsync(CreateUpdateRecipeIngredientDto input)
        {
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
            {
                throw new KeyNotFoundException("RecipeIngredient not found.");
            }

            return _mapper.Map<RecipeIngredientResponseDto>(recipeIngredient);
        }

        public async Task<RecipeIngredientResponseDto> UpdateAsync(
            int id,
            CreateUpdateRecipeIngredientDto input)
        {
            var recipeIngredient = await _recipeIngredientRepository.GetByIdAsync(id);

            if (recipeIngredient == null)
            {
                throw new KeyNotFoundException("RecipeIngredient not found.");
            }

            _mapper.Map(input, recipeIngredient);

            var updatedRecipeIngredient = await _recipeIngredientRepository.UpdateAsync(recipeIngredient);

            return _mapper.Map<RecipeIngredientResponseDto>(updatedRecipeIngredient);
        }

        public async Task DeleteAsync(int id)
        {
            var recipeIngredient = await _recipeIngredientRepository.GetByIdAsync(id);

            if (recipeIngredient == null)
            {
                throw new KeyNotFoundException("RecipeIngredient not found.");
            }

            await _recipeIngredientRepository.DeleteAsync(recipeIngredient);
        }
    }
}
