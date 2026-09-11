using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.StockTransactions;

public class StockTransactionRepository : IStockTransactionRepository
{
    private readonly DataContext _context;

    public StockTransactionRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<StockTransaction> CreateAsync(StockTransaction stockTransaction)
    {
        _context.StockTransactions.Add(stockTransaction);
        await _context.SaveChangesAsync();
        return stockTransaction;
    }

    public async Task<List<StockTransaction>> GetAllAsync()
    {
        return await _context.StockTransactions
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<StockTransaction?> GetByIdAsync(int id)
    {
        return await _context.StockTransactions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<StockTransaction> UpdateAsync(StockTransaction stockTransaction)
    {
        _context.StockTransactions.Update(stockTransaction);
        await _context.SaveChangesAsync();
        return stockTransaction;
    }

    public async Task DeleteAsync(StockTransaction stockTransaction)
    {
        stockTransaction.IsDeleted = true;
        stockTransaction.UpdatedDate = DateTime.UtcNow;
        _context.StockTransactions.Update(stockTransaction);
        await _context.SaveChangesAsync();
    }
}