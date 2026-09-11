using Domain.Entities;

namespace Infrastructure.Repositories.OrderItems;

public interface IOrderItemRepository
{
    Task<OrderItem> CreateAsync(OrderItem orderItem);
    Task<List<OrderItem>> GetAllAsync();
    Task<OrderItem?> GetByIdAsync(int id);
    Task<OrderItem> UpdateAsync(OrderItem orderItem);
    Task DeleteAsync(OrderItem orderItem);
}