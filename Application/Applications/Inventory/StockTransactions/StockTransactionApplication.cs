using Application.Dtos.StockTransactions;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.StockTransactions;

namespace Application.Applications.StockTransactions;

public class StockTransactionApplication : IStockTransactionApplication
{
    private readonly IStockTransactionRepository _stockTransactionRepository;
    private readonly IMapper _mapper;

    public StockTransactionApplication(
        IStockTransactionRepository stockTransactionRepository,
        IMapper mapper)
    {
        _stockTransactionRepository = stockTransactionRepository;
        _mapper = mapper;
    }

    public async Task<StockTransactionResponseDto> CreateAsync(CreateUpdateStockTransactionDto input)
    {
        var stockTransaction = _mapper.Map<StockTransaction>(input);
        var createdStockTransaction = await _stockTransactionRepository.CreateAsync(stockTransaction);
        return _mapper.Map<StockTransactionResponseDto>(createdStockTransaction);
    }

    public async Task<List<StockTransactionResponseDto>> GetAllAsync()
    {
        var transactions = await _stockTransactionRepository.GetAllAsync();
        return _mapper.Map<List<StockTransactionResponseDto>>(transactions);
    }

    public async Task<StockTransactionResponseDto> GetByIdAsync(int id)
    {
        var transaction = await _stockTransactionRepository.GetByIdAsync(id);

        if (transaction == null)
            throw new KeyNotFoundException("Stock transaction not found.");

        return _mapper.Map<StockTransactionResponseDto>(transaction);
    }

    public async Task<StockTransactionResponseDto> UpdateAsync(
        int id,
        CreateUpdateStockTransactionDto input)
    {
        var transaction = await _stockTransactionRepository.GetByIdAsync(id);

        if (transaction == null)
            throw new KeyNotFoundException("Stock transaction not found.");

        _mapper.Map(input, transaction);
        transaction.UpdatedDate = DateTime.UtcNow;

        var updatedTransaction = await _stockTransactionRepository.UpdateAsync(transaction);
        return _mapper.Map<StockTransactionResponseDto>(updatedTransaction);
    }

    public async Task DeleteAsync(int id)
    {
        var transaction = await _stockTransactionRepository.GetByIdAsync(id);

        if (transaction == null)
            throw new KeyNotFoundException("Stock transaction not found.");

        await _stockTransactionRepository.DeleteAsync(transaction);
    }
}