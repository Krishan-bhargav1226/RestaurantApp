using Application.Dtos.BranchProducts;

namespace Application.Applications.BranchProducts
{
    public interface IBranchProductApplication
    {
        Task<BranchProductResponseDto> CreateAsync(CreateUpdateBranchProductDto input);
        Task<List<BranchProductResponseDto>> GetAllAsync();
        Task<BranchProductResponseDto> GetByIdAsync(int id);
        Task<BranchProductResponseDto> UpdateAsync(int id, CreateUpdateBranchProductDto input);
        Task DeleteAsync(int id);
    }
}
