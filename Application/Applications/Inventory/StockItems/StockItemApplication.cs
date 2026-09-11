using Application.Dtos.StockItems;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.StockItems;

namespace Application.Applications.StockItems;

public class StockItemApplication : IStockItemApplication
{
    private readonly IStockItemRepository _stockItemRepository;
    private readonly IMapper _mapper;

    public StockItemApplication(IStockItemRepository stockItemRepository, IMapper mapper)
    {
        _stockItemRepository = stockItemRepository;
        _mapper = mapper;
    }

    public async Task<StockItemResponseDto> CreateAsync(CreateUpdateStockItemDto input)
    {
        var stockItem = _mapper.Map<StockItem>(input);
        var createdStockItem = await _stockItemRepository.CreateAsync(stockItem);
        return _mapper.Map<StockItemResponseDto>(createdStockItem);
    }

    public async Task<List<StockItemResponseDto>> GetAllAsync()
    {
        var stockItems = await _stockItemRepository.GetAllAsync();
        return _mapper.Map<List<StockItemResponseDto>>(stockItems);
    }

    public async Task<StockItemResponseDto> GetByIdAsync(int id)
    {
        var stockItem = await _stockItemRepository.GetByIdAsync(id);

        if (stockItem == null)
            throw new KeyNotFoundException("Stock item not found.");

        return _mapper.Map<StockItemResponseDto>(stockItem);
    }

    public async Task<StockItemResponseDto> UpdateAsync(int id, CreateUpdateStockItemDto input)
    {
        var stockItem = await _stockItemRepository.GetByIdAsync(id);

        if (stockItem == null)
            throw new KeyNotFoundException("Stock item not found.");

        _mapper.Map(input, stockItem);
        stockItem.UpdatedDate = DateTime.UtcNow;

        var updatedStockItem = await _stockItemRepository.UpdateAsync(stockItem);
        return _mapper.Map<StockItemResponseDto>(updatedStockItem);
    }

    public async Task DeleteAsync(int id)
    {
        var stockItem = await _stockItemRepository.GetByIdAsync(id);

        if (stockItem == null)
            throw new KeyNotFoundException("Stock item not found.");

        await _stockItemRepository.DeleteAsync(stockItem);
    }
}