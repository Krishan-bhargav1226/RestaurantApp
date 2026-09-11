using Domain.Entities;

namespace Infrastructure.Repositories.PasswordResetOTPs;

public interface IPasswordResetOTPRepository
{
    Task<PasswordResetOTP> CreateAsync(PasswordResetOTP passwordResetOTP);
    Task<List<PasswordResetOTP>> GetAllAsync();
    Task<PasswordResetOTP?> GetByIdAsync(int id);
    Task<PasswordResetOTP> UpdateAsync(PasswordResetOTP passwordResetOTP);
    Task DeleteAsync(PasswordResetOTP passwordResetOTP);
}