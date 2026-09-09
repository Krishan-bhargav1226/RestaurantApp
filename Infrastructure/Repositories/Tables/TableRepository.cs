using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Tables
{
    public class TableRepository : ITableRepository
    {
        private readonly DataContext _context;

        public TableRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Table> CreateAsync(Table table)
        {
            _context.Set<Table>().Add(table);
            await _context.SaveChangesAsync();

            return table;
        }

        public async Task<List<Table>> GetAllAsync()
        {
            return await _context.Set<Table>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Table?> GetByIdAsync(int id)
        {
            return await _context.Set<Table>()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ExistsForBranchAsync(
            int branchId,
            string tableNumber,
            int? excludeId = null)
        {
            return await _context.Set<Table>()
                .AnyAsync(x =>
                    x.BranchId == branchId &&
                    x.TableNumber == tableNumber &&
                    (!excludeId.HasValue || x.Id != excludeId.Value));
        }

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
}