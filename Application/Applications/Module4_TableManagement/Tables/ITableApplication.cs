using Application.Dtos.Tables;

namespace Application.Applications.Tables;

public interface ITableApplication
{
    Task<TableResponseDto> CreateAsync(CreateUpdateTableDto input);
    Task<List<TableResponseDto>> GetAllAsync();
    Task<TableResponseDto> GetByIdAsync(int id);
    Task<TableResponseDto> UpdateAsync(int id, CreateUpdateTableDto input);
    Task DeleteAsync(int id);
}
