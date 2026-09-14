using Domain.Entities;

namespace Infrastructure.Repositories.TableSessions;

public interface ITableSessionRepository
{
    Task<TableSession> CreateAsync(TableSession session);

    Task<List<TableSession>> GetAllAsync();

    Task<TableSession?> GetByIdAsync(int id);

    Task<TableSession> UpdateAsync(TableSession session);

    Task DeleteAsync(TableSession session);
}