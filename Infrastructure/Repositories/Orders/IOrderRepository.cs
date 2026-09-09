using Domain.Entities;

namespace Infrastructure.Repositories.Orders;

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order);
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<Order> UpdateAsync(Order order);
    Task DeleteAsync(Order order);
}
