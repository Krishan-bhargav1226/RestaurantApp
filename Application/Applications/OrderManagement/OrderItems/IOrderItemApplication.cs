using Application.Dtos.OrderItems;

namespace Application.Applications.OrderItems;

public interface IOrderItemApplication
{
    Task<OrderItemResponseDto> CreateAsync(CreateUpdateOrderItemCrudDto input);
    Task<List<OrderItemResponseDto>> GetAllAsync();
    Task<OrderItemResponseDto> GetByIdAsync(int id);
    Task<OrderItemResponseDto> UpdateAsync(int id, CreateUpdateOrderItemCrudDto input);
    Task DeleteAsync(int id);
}