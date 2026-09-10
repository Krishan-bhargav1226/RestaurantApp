using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserAsync(string emailOrPhone)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.IsActive && (x.Email == emailOrPhone || x.Phone == emailOrPhone));
        }

        public async Task<Customer?> GetCustomerAsync(string emailOrPhone)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.IsActive && (x.Email == emailOrPhone || x.Phone == emailOrPhone));
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshTokenHash)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.IsActive && x.RefreshTokenHash == refreshTokenHash && x.RefreshTokenExpiry > DateTime.UtcNow);
        }

        public async Task<Customer?> GetCustomerByRefreshTokenAsync(string refreshTokenHash)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.IsActive && x.RefreshTokenHash == refreshTokenHash && x.RefreshTokenExpiry > DateTime.UtcNow);
        }

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

        public async Task<PasswordResetOTP> CreatePasswordResetOTPAsync(PasswordResetOTP resetOtp)
        {
            _context.PasswordResetOTPs.Add(resetOtp);
            await _context.SaveChangesAsync();
            return resetOtp;
        }

        public async Task<PasswordResetOTP?> GetPasswordResetOTPAsync(string phoneOrEmail, string otpHash, string purpose)
        {
            return await _context.PasswordResetOTPs
                .Where(x => x.PhoneOrEmail == phoneOrEmail && x.OTPHash == otpHash && x.Purpose == purpose && !x.IsUsed)
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<PasswordResetOTP> UpdatePasswordResetOTPAsync(PasswordResetOTP resetOtp)
        {
            _context.PasswordResetOTPs.Update(resetOtp);
            await _context.SaveChangesAsync();
            return resetOtp;
        }
    }
}