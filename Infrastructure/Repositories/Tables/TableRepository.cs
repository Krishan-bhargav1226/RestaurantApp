using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Tables;

public class TableRepository : ITableRepository
{
    private readonly DataContext _context;
    public TableRepository(DataContext context) => _context = context;

    public async Task<Table> CreateAsync(Table table)
    {
        _context.Set<Table>().Add(table);
        await _context.SaveChangesAsync();
        return table;
    }

    public Task<List<Table>> GetAllAsync() => _context.Set<Table>().AsNoTracking().ToListAsync();

    public Task<Table?> GetByIdAsync(int id) => _context.Set<Table>().FirstOrDefaultAsync(x => x.Id == id);

    public Task<bool> ExistsForBranchAsync(int branchId, string tableNumber, int? excludeId = null) =>
        _context.Set<Table>().AnyAsync(x => x.BranchId == branchId && x.TableNumber == tableNumber && (!excludeId.HasValue || x.Id != excludeId.Value));

    public async Task<Table> UpdateAsync(Table table)
    {
        _context.Set<Table>().Update(table);
        await _context.SaveChangesAsync();
        return table;
    }

    public async Task DeleteAsync(Table table)
    {
        table.IsDeleted = true;
        table.UpdatedDate = DateTime.UtcNow;
        _context.Set<Table>().Update(table);
        await _context.SaveChangesAsync();
    }
}
