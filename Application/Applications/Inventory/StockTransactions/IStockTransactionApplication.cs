using Application.Dtos.StockTransactions;

namespace Application.Applications.StockTransactions;

public interface IStockTransactionApplication
{
    Task<StockTransactionResponseDto> CreateAsync(CreateUpdateStockTransactionDto input);
    Task<List<StockTransactionResponseDto>> GetAllAsync();
    Task<StockTransactionResponseDto> GetByIdAsync(int id);
    Task<StockTransactionResponseDto> UpdateAsync(int id, CreateUpdateStockTransactionDto input);
    Task DeleteAsync(int id);
}