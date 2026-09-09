using Domain.Entities;

namespace Infrastructure.Repositories.Products
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product);
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> UpdateAsync(Product product);
        Task DeleteAsync(Product product);
    }
}
