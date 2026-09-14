using Application.Dtos.TableSessions;

namespace Application.Applications.TableSessions;

public interface ITableSessionApplication
{
    Task<TableSessionResponseDto> CreateAsync(
        CreateUpdateTableSessionDto input);

    Task<List<TableSessionResponseDto>> GetAllAsync();

    Task<TableSessionResponseDto?> GetByIdAsync(int id);

    Task<TableSessionResponseDto?> UpdateAsync(
        int id,
        CreateUpdateTableSessionDto input);

    Task<bool> DeleteAsync(int id);
}