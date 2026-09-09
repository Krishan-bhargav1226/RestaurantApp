using Domain.Entities;

namespace Infrastructure.Repositories.TableStatusHistories;

public interface ITableStatusHistoryRepository
{
    Task<TableStatusHistory> CreateAsync(TableStatusHistory history);
    Task<List<TableStatusHistory>> GetAllAsync();
    Task<TableStatusHistory?> GetByIdAsync(int id);
    Task<TableStatusHistory> UpdateAsync(TableStatusHistory history);
    Task DeleteAsync(TableStatusHistory history);
}
