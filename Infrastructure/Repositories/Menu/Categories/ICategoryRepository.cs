using Domain.Entities;

namespace Infrastructure.Repositories.Categories
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> UpdateAsync(Category category);
        Task DeleteAsync(Category category);
    }
}
