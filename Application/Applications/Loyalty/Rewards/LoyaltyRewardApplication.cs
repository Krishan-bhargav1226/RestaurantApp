using Application.Dtos.LoyaltyRewards;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.LoyaltyRewards;

namespace Application.Applications.LoyaltyRewards;

public class LoyaltyRewardApplication : ILoyaltyRewardApplication
{
    private readonly ILoyaltyRewardRepository _loyaltyRewardRepository;
    private readonly IMapper _mapper;

    public LoyaltyRewardApplication(
        ILoyaltyRewardRepository loyaltyRewardRepository,
        IMapper mapper)
    {
        _loyaltyRewardRepository = loyaltyRewardRepository;
        _mapper = mapper;
    }

    public async Task<LoyaltyRewardResponseDto> CreateAsync(CreateUpdateLoyaltyRewardDto input)
    {
        var loyaltyReward = _mapper.Map<LoyaltyReward>(input);
        var createdLoyaltyReward = await _loyaltyRewardRepository.CreateAsync(loyaltyReward);

        return _mapper.Map<LoyaltyRewardResponseDto>(createdLoyaltyReward);
    }

    public async Task<List<LoyaltyRewardResponseDto>> GetAllAsync()
    {
        var rewards = await _loyaltyRewardRepository.GetAllAsync();
        return _mapper.Map<List<LoyaltyRewardResponseDto>>(rewards);
    }

    public async Task<LoyaltyRewardResponseDto> GetByIdAsync(int id)
    {
        var reward = await _loyaltyRewardRepository.GetByIdAsync(id);

        if (reward == null)
            throw new KeyNotFoundException("Loyalty reward not found.");

        return _mapper.Map<LoyaltyRewardResponseDto>(reward);
    }

    public async Task<LoyaltyRewardResponseDto> UpdateAsync(int id, CreateUpdateLoyaltyRewardDto input)
    {
        var reward = await _loyaltyRewardRepository.GetByIdAsync(id);

        if (reward == null)
            throw new KeyNotFoundException("Loyalty reward not found.");

        _mapper.Map(input, reward);
        reward.UpdatedDate = DateTime.UtcNow;

        var updatedReward = await _loyaltyRewardRepository.UpdateAsync(reward);
        return _mapper.Map<LoyaltyRewardResponseDto>(updatedReward);
    }

    public async Task DeleteAsync(int id)
    {
        var reward = await _loyaltyRewardRepository.GetByIdAsync(id);

        if (reward == null)
            throw new KeyNotFoundException("Loyalty reward not found.");

        await _loyaltyRewardRepository.DeleteAsync(reward);
    }
}