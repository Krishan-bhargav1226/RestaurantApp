using Application.Dtos.Ingredients;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Ingredients;

namespace Application.Applications.Ingredients
{
    public class IngredientApplication : IIngredientApplication
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _mapper;

        public IngredientApplication(
            IIngredientRepository ingredientRepository,
            IMapper mapper)
        {
            _ingredientRepository = ingredientRepository;
            _mapper = mapper;
        }

        public async Task<IngredientResponseDto> CreateAsync(CreateUpdateIngredientDto input)
        {
            var ingredient = _mapper.Map<Ingredient>(input);

            var createdIngredient = await _ingredientRepository.CreateAsync(ingredient);

            return _mapper.Map<IngredientResponseDto>(createdIngredient);
        }

        public async Task<List<IngredientResponseDto>> GetAllAsync()
        {
            var ingredients = await _ingredientRepository.GetAllAsync();

            return _mapper.Map<List<IngredientResponseDto>>(ingredients);
        }

        public async Task<IngredientResponseDto> GetByIdAsync(int id)
        {
            var ingredient = await _ingredientRepository.GetByIdAsync(id);

            if (ingredient == null)
            {
                throw new KeyNotFoundException("Ingredient not found.");
            }

            return _mapper.Map<IngredientResponseDto>(ingredient);
        }

        public async Task<IngredientResponseDto> UpdateAsync(
            int id,
            CreateUpdateIngredientDto input)
        {
            var ingredient = await _ingredientRepository.GetByIdAsync(id);

            if (ingredient == null)
            {
                throw new KeyNotFoundException("Ingredient not found.");
            }

            _mapper.Map(input, ingredient);

            var updatedIngredient = await _ingredientRepository.UpdateAsync(ingredient);

            return _mapper.Map<IngredientResponseDto>(updatedIngredient);
        }

        public async Task DeleteAsync(int id)
        {
            var ingredient = await _ingredientRepository.GetByIdAsync(id);

            if (ingredient == null)
            {
                throw new KeyNotFoundException("Ingredient not found.");
            }

            await _ingredientRepository.DeleteAsync(ingredient);
        }
    }
}
