using Domain.Entities;

namespace Infrastructure.Repositories.Users;

public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(User user);
}
