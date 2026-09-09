using Application.Dtos.Orders;

namespace Application.Applications.Orders;

public interface IOrderApplication
{
    Task<OrderResponseDto> CreateAsync(CreateUpdateOrderDto input);
    Task<List<OrderResponseDto>> GetAllAsync();
    Task<OrderResponseDto> GetByIdAsync(int id);
    Task<OrderResponseDto> UpdateAsync(int id, CreateUpdateOrderDto input);
    Task DeleteAsync(int id);
}
