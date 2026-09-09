using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Auth;

public class AuthRepository : IAuthRepository
{
    private readonly DataContext _context;
    public AuthRepository(DataContext context) => _context = context;

    public Task<User?> GetUserAsync(string emailOrPhone) => _context.Users.FirstOrDefaultAsync(x => !x.IsDeleted && x.IsActive && (x.Email == emailOrPhone || x.Phone == emailOrPhone));
    public Task<Customer?> GetCustomerAsync(string emailOrPhone) => _context.Customers.FirstOrDefaultAsync(x => !x.IsDeleted && x.IsActive && (x.Email == emailOrPhone || x.Phone == emailOrPhone));
    public Task<bool> UserEmailOrPhoneExistsAsync(string email, string phone) => _context.Users.AnyAsync(x => !x.IsDeleted && (x.Email == email || x.Phone == phone));
    public Task<bool> CustomerEmailOrPhoneExistsAsync(string email, string phone) => _context.Customers.AnyAsync(x => !x.IsDeleted && (x.Email == email || x.Phone == phone));

    public async Task<User> CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }
}
