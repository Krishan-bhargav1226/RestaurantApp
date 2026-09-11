using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.OrderItems;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly DataContext _context;

    public OrderItemRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<OrderItem> CreateAsync(OrderItem orderItem)
    {
        _context.OrderItems.Add(orderItem);
        await _context.SaveChangesAsync();
        return orderItem;
    }

    public async Task<List<OrderItem>> GetAllAsync()
    {
        return await _context.OrderItems
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<OrderItem?> GetByIdAsync(int id)
    {
        return await _context.OrderItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<OrderItem> UpdateAsync(OrderItem orderItem)
    {
        _context.OrderItems.Update(orderItem);
        await _context.SaveChangesAsync();
        return orderItem;
    }

    public async Task DeleteAsync(OrderItem orderItem)
    {
        orderItem.IsDeleted = true;
        orderItem.UpdatedDate = DateTime.UtcNow;
        _context.OrderItems.Update(orderItem);
        await _context.SaveChangesAsync();
    }
}