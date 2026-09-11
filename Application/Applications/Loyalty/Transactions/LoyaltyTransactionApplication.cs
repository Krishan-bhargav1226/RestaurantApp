using Application.Dtos.LoyaltyTransactions;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.LoyaltyTransactions;

namespace Application.Applications.LoyaltyTransactions;

public class LoyaltyTransactionApplication : ILoyaltyTransactionApplication
{
    private readonly ILoyaltyTransactionRepository _loyaltyTransactionRepository;
    private readonly IMapper _mapper;

    public LoyaltyTransactionApplication(
        ILoyaltyTransactionRepository loyaltyTransactionRepository,
        IMapper mapper)
    {
        _loyaltyTransactionRepository = loyaltyTransactionRepository;
        _mapper = mapper;
    }

    public async Task<LoyaltyTransactionResponseDto> CreateAsync(CreateUpdateLoyaltyTransactionDto input)
    {
        var loyaltyTransaction = _mapper.Map<LoyaltyTransaction>(input);
        var createdLoyaltyTransaction = await _loyaltyTransactionRepository.CreateAsync(loyaltyTransaction);

        return _mapper.Map<LoyaltyTransactionResponseDto>(createdLoyaltyTransaction);
    }

    public async Task<List<LoyaltyTransactionResponseDto>> GetAllAsync()
    {
        var transactions = await _loyaltyTransactionRepository.GetAllAsync();
        return _mapper.Map<List<LoyaltyTransactionResponseDto>>(transactions);
    }

    public async Task<LoyaltyTransactionResponseDto> GetByIdAsync(int id)
    {
        var transaction = await _loyaltyTransactionRepository.GetByIdAsync(id);

        if (transaction == null)
            throw new KeyNotFoundException("Loyalty transaction not found.");

        return _mapper.Map<LoyaltyTransactionResponseDto>(transaction);
    }

    public async Task<LoyaltyTransactionResponseDto> UpdateAsync(int id, CreateUpdateLoyaltyTransactionDto input)
    {
        var transaction = await _loyaltyTransactionRepository.GetByIdAsync(id);

        if (transaction == null)
            throw new KeyNotFoundException("Loyalty transaction not found.");

        _mapper.Map(input, transaction);
        transaction.UpdatedDate = DateTime.UtcNow;

        var updatedTransaction = await _loyaltyTransactionRepository.UpdateAsync(transaction);
        return _mapper.Map<LoyaltyTransactionResponseDto>(updatedTransaction);
    }

    public async Task DeleteAsync(int id)
    {
        var transaction = await _loyaltyTransactionRepository.GetByIdAsync(id);

        if (transaction == null)
            throw new KeyNotFoundException("Loyalty transaction not found.");

        await _loyaltyTransactionRepository.DeleteAsync(transaction);
    }
}