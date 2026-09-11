using Domain.Entities;

namespace Infrastructure.Repositories.StockTransactions;

public interface IStockTransactionRepository
{
    Task<StockTransaction> CreateAsync(StockTransaction stockTransaction);
    Task<List<StockTransaction>> GetAllAsync();
    Task<StockTransaction?> GetByIdAsync(int id);
    Task<StockTransaction> UpdateAsync(StockTransaction stockTransaction);
    Task DeleteAsync(StockTransaction stockTransaction);
}