using Application.Dtos.LoyaltyRewards;
using Application.Dtos.LoyaltyTransactions;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping;

public class LoyaltyProfile : Profile
{
    public LoyaltyProfile()
    {
        CreateMap<CreateUpdateLoyaltyTransactionDto, LoyaltyTransaction>();
        CreateMap<LoyaltyTransaction, LoyaltyTransactionResponseDto>();

        CreateMap<CreateUpdateLoyaltyRewardDto, LoyaltyReward>();
        CreateMap<LoyaltyReward, LoyaltyRewardResponseDto>();
    }
}