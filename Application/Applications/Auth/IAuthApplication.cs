using Application.Dtos.Auth;

namespace Application.Applications.Auth;

public interface IAuthApplication
{
    Task<AuthResponseDto> RegisterUserAsync(RegisterDto input);
    Task<AuthResponseDto> RegisterCustomerAsync(RegisterDto input);
    Task<AuthResponseDto> LoginAsync(LoginDto input);
}
