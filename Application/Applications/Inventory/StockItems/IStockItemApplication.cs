using Application.Dtos.StockItems;

namespace Application.Applications.StockItems;

public interface IStockItemApplication
{
    Task<StockItemResponseDto> CreateAsync(CreateUpdateStockItemDto input);
    Task<List<StockItemResponseDto>> GetAllAsync();
    Task<StockItemResponseDto> GetByIdAsync(int id);
    Task<StockItemResponseDto> UpdateAsync(int id, CreateUpdateStockItemDto input);
    Task DeleteAsync(int id);
}