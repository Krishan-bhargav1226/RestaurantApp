using Domain.Entities;

namespace Infrastructure.Repositories.Auth;

public interface IAuthRepository
{
    Task<User?> GetUserAsync(string emailOrPhone);
    Task<Customer?> GetCustomerAsync(string emailOrPhone);
    Task<bool> UserEmailOrPhoneExistsAsync(string email, string phone);
    Task<bool> CustomerEmailOrPhoneExistsAsync(string email, string phone);
    Task<User> CreateUserAsync(User user);
    Task<Customer> CreateCustomerAsync(Customer customer);
}
