using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.CustomerAddresses;

public class CustomerAddressRepository : ICustomerAddressRepository
{
    private readonly DataContext _context;
    public CustomerAddressRepository(DataContext context) => _context = context;
    public Task<List<CustomerAddress>> GetByCustomerIdAsync(int customerId) => _context.CustomerAddresses.Where(x => x.CustomerId == customerId).ToListAsync();
    public Task<CustomerAddress?> GetByIdAsync(int id) => _context.CustomerAddresses.FirstOrDefaultAsync(x => x.Id == id);
    public async Task<CustomerAddress> CreateAsync(CustomerAddress address) { _context.CustomerAddresses.Add(address); await _context.SaveChangesAsync(); return address; }
    public async Task<CustomerAddress> UpdateAsync(CustomerAddress address) { _context.CustomerAddresses.Update(address); await _context.SaveChangesAsync(); return address; }
    public async Task DeleteAsync(CustomerAddress address) { address.IsDeleted = true; address.UpdatedDate = DateTime.UtcNow; await _context.SaveChangesAsync(); }
}
