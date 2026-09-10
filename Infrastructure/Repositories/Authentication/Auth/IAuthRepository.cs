using Domain.Entities;

namespace Infrastructure.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<User?> GetUserAsync(string email);
        Task<Customer?> GetCustomerAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByRefreshTokenAsync(string refreshTokenHash);
        Task<Customer?> GetCustomerByRefreshTokenAsync(string refreshTokenHash);
        Task<User> CreateUserAsync(User user);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<PasswordResetOTP> CreatePasswordResetOTPAsync(PasswordResetOTP resetOtp);
        Task<PasswordResetOTP?> GetPasswordResetOTPAsync(string email, string otpHash, string purpose);
        Task<User> UpdateUserAsync(User user);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task<PasswordResetOTP> UpdatePasswordResetOTPAsync(PasswordResetOTP resetOtp);
    }
}