using Application.Dtos.Auth.User;

namespace Application.Applications.Auth.User;

public interface IUserAuthApplication
{
    Task<UserRegistrationResponseDto> RegisterAsync(RegisterUserDto input);
    Task<string> GenerateRegistrationOtpAsync(string email);
    Task<UserRegistrationResponseDto> VerifyRegistrationOtpAsync(VerifyUserOtpDto input);
    Task<UserAuthResponseDto> LoginAsync(LoginUserDto input);
    Task<UserAuthResponseDto> RefreshTokenAsync(RefreshUserTokenDto input);
    Task<string?> ForgotPasswordAsync(string email);
    Task ResetPasswordAsync(ResetUserPasswordDto input);
    Task<UserRegistrationResponseDto> AssignRoleAsync(int userId, AssignRoleDto input);
}