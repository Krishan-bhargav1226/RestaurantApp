using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.TableStatusHistories;

public class TableStatusHistoryRepository : ITableStatusHistoryRepository
{
    private readonly DataContext _context;
    public TableStatusHistoryRepository(DataContext context) => _context = context;

    public async Task<TableStatusHistory> CreateAsync(TableStatusHistory history)
    {
        _context.Set<TableStatusHistory>().Add(history);
        await _context.SaveChangesAsync();
        return history;
    }

    public Task<List<TableStatusHistory>> GetAllAsync() => _context.Set<TableStatusHistory>().AsNoTracking().ToListAsync();

    public Task<TableStatusHistory?> GetByIdAsync(int id) => _context.Set<TableStatusHistory>().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<TableStatusHistory> UpdateAsync(TableStatusHistory history)
    {
        _context.Set<TableStatusHistory>().Update(history);
        await _context.SaveChangesAsync();
        return history;
    }

    public async Task DeleteAsync(TableStatusHistory history)
    {
        history.IsDeleted = true;
        history.UpdatedDate = DateTime.UtcNow;
        _context.Set<TableStatusHistory>().Update(history);
        await _context.SaveChangesAsync();
    }
}
