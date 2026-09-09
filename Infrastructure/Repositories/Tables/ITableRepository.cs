using Domain.Entities;

namespace Infrastructure.Repositories.Tables;

public interface ITableRepository
{
    Task<Table> CreateAsync(Table table);
    Task<List<Table>> GetAllAsync();
    Task<Table?> GetByIdAsync(int id);
    Task<bool> ExistsForBranchAsync(int branchId, string tableNumber, int? excludeId = null);
    Task<Table> UpdateAsync(Table table);
    Task DeleteAsync(Table table);
}
