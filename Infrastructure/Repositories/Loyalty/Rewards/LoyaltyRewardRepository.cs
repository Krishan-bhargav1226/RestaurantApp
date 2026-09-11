using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.LoyaltyRewards;

public class LoyaltyRewardRepository : ILoyaltyRewardRepository
{
    private readonly DataContext _context;

    public LoyaltyRewardRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<LoyaltyReward> CreateAsync(LoyaltyReward loyaltyReward)
    {
        _context.LoyaltyRewards.Add(loyaltyReward);
        await _context.SaveChangesAsync();
        return loyaltyReward;
    }

    public async Task<List<LoyaltyReward>> GetAllAsync()
    {
        return await _context.LoyaltyRewards
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<LoyaltyReward?> GetByIdAsync(int id)
    {
        return await _context.LoyaltyRewards
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<LoyaltyReward> UpdateAsync(LoyaltyReward loyaltyReward)
    {
        _context.LoyaltyRewards.Update(loyaltyReward);
        await _context.SaveChangesAsync();
        return loyaltyReward;
    }

    public async Task DeleteAsync(LoyaltyReward loyaltyReward)
    {
        loyaltyReward.IsDeleted = true;
        loyaltyReward.UpdatedDate = DateTime.UtcNow;
        _context.LoyaltyRewards.Update(loyaltyReward);
        await _context.SaveChangesAsync();
    }
}