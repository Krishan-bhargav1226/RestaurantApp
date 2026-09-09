using Application.Dtos.TableStatusHistories;

namespace Application.Applications.TableStatusHistories;

public interface ITableStatusHistoryApplication
{
    Task<TableStatusHistoryResponseDto> CreateAsync(CreateUpdateTableStatusHistoryDto input);
    Task<List<TableStatusHistoryResponseDto>> GetAllAsync();
    Task<TableStatusHistoryResponseDto> GetByIdAsync(int id);
    Task<TableStatusHistoryResponseDto> UpdateAsync(int id, CreateUpdateTableStatusHistoryDto input);
    Task DeleteAsync(int id);
}
