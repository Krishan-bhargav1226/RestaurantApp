using Domain.Entities;

namespace Infrastructure.Repositories.Customers;

public interface ICustomerRepository
{
    Task<Customer> CreateAsync(Customer customer);
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task<Customer> UpdateAsync(Customer customer);
    Task DeleteAsync(Customer customer);
}
