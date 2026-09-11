using Application.Dtos.StockItems;
using Application.Dtos.StockTransactions;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping;

public class InventoryProfile : Profile
{
    public InventoryProfile()
    {
        CreateMap<CreateUpdateStockItemDto, StockItem>();
        CreateMap<StockItem, StockItemResponseDto>();

        CreateMap<CreateUpdateStockTransactionDto, StockTransaction>();
        CreateMap<StockTransaction, StockTransactionResponseDto>();
    }
}