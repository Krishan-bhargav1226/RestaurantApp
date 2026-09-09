using Application.Dtos.Categories;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Categories;

namespace Application.Applications.Categories
{
    public class CategoryApplication : ICategoryApplication
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryApplication(
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryResponseDto> CreateAsync(CreateUpdateCategoryDto input)
        {
            var category = _mapper.Map<Category>(input);

            var createdCategory = await _categoryRepository.CreateAsync(category);

            return _mapper.Map<CategoryResponseDto>(createdCategory);
        }

        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return _mapper.Map<List<CategoryResponseDto>>(categories);
        }

        public async Task<CategoryResponseDto> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            return _mapper.Map<CategoryResponseDto>(category);
        }

        public async Task<CategoryResponseDto> UpdateAsync(
            int id,
            CreateUpdateCategoryDto input)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            _mapper.Map(input, category);

            var updatedCategory = await _categoryRepository.UpdateAsync(category);

            return _mapper.Map<CategoryResponseDto>(updatedCategory);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            await _categoryRepository.DeleteAsync(category);
        }
    }
}