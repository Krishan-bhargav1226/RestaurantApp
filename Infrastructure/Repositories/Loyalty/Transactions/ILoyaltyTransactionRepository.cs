using Domain.Entities;

namespace Infrastructure.Repositories.LoyaltyTransactions;

public interface ILoyaltyTransactionRepository
{
    Task<LoyaltyTransaction> CreateAsync(LoyaltyTransaction loyaltyTransaction);
    Task<List<LoyaltyTransaction>> GetAllAsync();
    Task<LoyaltyTransaction?> GetByIdAsync(int id);
    Task<LoyaltyTransaction> UpdateAsync(LoyaltyTransaction loyaltyTransaction);
    Task DeleteAsync(LoyaltyTransaction loyaltyTransaction);
}