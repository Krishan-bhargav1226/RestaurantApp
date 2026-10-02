using Application.Dtos.Branches;

namespace Application.Applications.Branches
{
    public interface IBranchApplication
    {
        Task<BranchResponseDto> CreateAsync(CreateUpdateBranchDto input);
        Task<List<BranchResponseDto>> GetAllAsync();
        Task<BranchResponseDto> GetByIdAsync(int id);

        Task<BranchResponseDto> UpdateAsync(int id, CreateUpdateBranchDto input);

        Task DeleteAsync(int id);


    }
}
