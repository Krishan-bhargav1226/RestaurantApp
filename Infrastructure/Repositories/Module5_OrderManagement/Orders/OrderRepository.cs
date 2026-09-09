using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(x => x.Items)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task DeleteAsync(Order order)
        {
            order.IsDeleted = true;
            order.UpdatedDate = DateTime.UtcNow;

            foreach (var item in order.Items.Where(x => !x.IsDeleted))
            {
                item.IsDeleted = true;
                item.UpdatedDate = DateTime.UtcNow;
            }

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}