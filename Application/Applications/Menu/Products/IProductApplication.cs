using Application.Dtos.Products;

namespace Application.Applications.Products
{
    public interface IProductApplication
    {
        Task<ProductResponseDto> CreateAsync(CreateUpdateProductDto input);
        Task<List<ProductResponseDto>> GetAllAsync();
        Task<ProductResponseDto> GetByIdAsync(int id);
        Task<ProductResponseDto> UpdateAsync(int id, CreateUpdateProductDto input);
        Task DeleteAsync(int id);
    }
}
