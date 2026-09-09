using Application.Dtos.Auth;

namespace Application.Applications.Auth
{
    public interface IAuthApplication
    {
        Task<AuthResponseDto> RegisterUserAsync(RegisterDto input);
        Task<AuthResponseDto> RegisterCustomerAsync(RegisterDto input);
        Task<AuthResponseDto> LoginAsync(LoginDto input);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto input);
        Task<string> ForgotPasswordAsync(string phoneOrEmail);
        Task ResetPasswordAsync(ResetPasswordDto input);
    }
}