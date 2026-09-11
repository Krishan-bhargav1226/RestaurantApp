using Application.Dtos.PasswordResetOTPs;

namespace Application.Applications.PasswordResetOTPs;

public interface IPasswordResetOTPApplication
{
    Task<PasswordResetOTPResponseDto> CreateAsync(CreateUpdatePasswordResetOTPDto input);
    Task<List<PasswordResetOTPResponseDto>> GetAllAsync();
    Task<PasswordResetOTPResponseDto> GetByIdAsync(int id);
    Task<PasswordResetOTPResponseDto> UpdateAsync(int id, CreateUpdatePasswordResetOTPDto input);
    Task DeleteAsync(int id);
}