using Domain.Entities;

namespace Infrastructure.Repositories.LoyaltyRewards;

public interface ILoyaltyRewardRepository
{
    Task<LoyaltyReward> CreateAsync(LoyaltyReward loyaltyReward);
    Task<List<LoyaltyReward>> GetAllAsync();
    Task<LoyaltyReward?> GetByIdAsync(int id);
    Task<LoyaltyReward> UpdateAsync(LoyaltyReward loyaltyReward);
    Task DeleteAsync(LoyaltyReward loyaltyReward);
}