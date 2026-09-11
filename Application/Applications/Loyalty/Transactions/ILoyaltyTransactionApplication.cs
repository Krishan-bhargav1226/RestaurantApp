using Application.Dtos.LoyaltyTransactions;

namespace Application.Applications.LoyaltyTransactions;

public interface ILoyaltyTransactionApplication
{
    Task<LoyaltyTransactionResponseDto> CreateAsync(CreateUpdateLoyaltyTransactionDto input);
    Task<List<LoyaltyTransactionResponseDto>> GetAllAsync();
    Task<LoyaltyTransactionResponseDto> GetByIdAsync(int id);
    Task<LoyaltyTransactionResponseDto> UpdateAsync(int id, CreateUpdateLoyaltyTransactionDto input);
    Task DeleteAsync(int id);
}