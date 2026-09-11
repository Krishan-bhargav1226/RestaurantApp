using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.StockItems;

public class StockItemRepository : IStockItemRepository
{
    private readonly DataContext _context;

    public StockItemRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<StockItem> CreateAsync(StockItem stockItem)
    {
        _context.StockItems.Add(stockItem);
        await _context.SaveChangesAsync();
        return stockItem;
    }

    public async Task<List<StockItem>> GetAllAsync()
    {
        return await _context.StockItems
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<StockItem?> GetByIdAsync(int id)
    {
        return await _context.StockItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<StockItem> UpdateAsync(StockItem stockItem)
    {
        _context.StockItems.Update(stockItem);
        await _context.SaveChangesAsync();
        return stockItem;
    }

    public async Task DeleteAsync(StockItem stockItem)
    {
        stockItem.IsDeleted = true;
        stockItem.UpdatedDate = DateTime.UtcNow;
        _context.StockItems.Update(stockItem);
        await _context.SaveChangesAsync();
    }
}