using Application.Dtos.LoyaltyRewards;

namespace Application.Applications.LoyaltyRewards;

public interface ILoyaltyRewardApplication
{
    Task<LoyaltyRewardResponseDto> CreateAsync(CreateUpdateLoyaltyRewardDto input);
    Task<List<LoyaltyRewardResponseDto>> GetAllAsync();
    Task<LoyaltyRewardResponseDto> GetByIdAsync(int id);
    Task<LoyaltyRewardResponseDto> UpdateAsync(int id, CreateUpdateLoyaltyRewardDto input);
    Task DeleteAsync(int id);
}