using Domain.Entities;

namespace Infrastructure.Repositories.CustomerAddresses;

public interface ICustomerAddressRepository
{
    Task<List<CustomerAddress>> GetByCustomerIdAsync(int customerId);
    Task<CustomerAddress?> GetByIdAsync(int id);
    Task<CustomerAddress> CreateAsync(CustomerAddress address);
    Task<CustomerAddress> UpdateAsync(CustomerAddress address);
    Task DeleteAsync(CustomerAddress address);
}
