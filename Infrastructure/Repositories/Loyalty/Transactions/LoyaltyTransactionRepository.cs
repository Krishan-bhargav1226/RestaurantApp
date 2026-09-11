using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.LoyaltyTransactions;

public class LoyaltyTransactionRepository : ILoyaltyTransactionRepository
{
    private readonly DataContext _context;

    public LoyaltyTransactionRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<LoyaltyTransaction> CreateAsync(LoyaltyTransaction loyaltyTransaction)
    {
        _context.LoyaltyTransactions.Add(loyaltyTransaction);
        await _context.SaveChangesAsync();
        return loyaltyTransaction;
    }

    public async Task<List<LoyaltyTransaction>> GetAllAsync()
    {
        return await _context.LoyaltyTransactions
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<LoyaltyTransaction?> GetByIdAsync(int id)
    {
        return await _context.LoyaltyTransactions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<LoyaltyTransaction> UpdateAsync(LoyaltyTransaction loyaltyTransaction)
    {
        _context.LoyaltyTransactions.Update(loyaltyTransaction);
        await _context.SaveChangesAsync();
        return loyaltyTransaction;
    }

    public async Task DeleteAsync(LoyaltyTransaction loyaltyTransaction)
    {
        loyaltyTransaction.IsDeleted = true;
        loyaltyTransaction.UpdatedDate = DateTime.UtcNow;
        _context.LoyaltyTransactions.Update(loyaltyTransaction);
        await _context.SaveChangesAsync();
    }
}