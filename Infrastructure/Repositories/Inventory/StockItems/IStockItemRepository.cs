using Domain.Entities;

namespace Infrastructure.Repositories.StockItems;

public interface IStockItemRepository
{
    Task<StockItem> CreateAsync(StockItem stockItem);
    Task<List<StockItem>> GetAllAsync();
    Task<StockItem?> GetByIdAsync(int id);
    Task<StockItem> UpdateAsync(StockItem stockItem);
    Task DeleteAsync(StockItem stockItem);
}