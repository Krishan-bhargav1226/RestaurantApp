using Application.Dtos.Categories;

namespace Application.Applications.Categories
{
    public interface ICategoryApplication
    {
        Task<CategoryResponseDto> CreateAsync(CreateUpdateCategoryDto input);
        Task<List<CategoryResponseDto>> GetAllAsync();
        Task<CategoryResponseDto> GetByIdAsync(int id);
        Task<CategoryResponseDto> UpdateAsync(int id, CreateUpdateCategoryDto input);
        Task DeleteAsync(int id);
    }
}
