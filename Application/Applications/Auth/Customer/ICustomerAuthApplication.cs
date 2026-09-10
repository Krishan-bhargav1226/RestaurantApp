using Application.Dtos.Auth.Customer;

namespace Application.Applications.Auth.Customer;

public interface ICustomerAuthApplication
{
    Task<CustomerRegistrationResponseDto> RegisterAsync(RegisterCustomerDto input);
    Task<string> GenerateRegistrationOtpAsync(string email);
    Task<CustomerRegistrationResponseDto> VerifyRegistrationOtpAsync(VerifyCustomerOtpDto input);
    Task<CustomerAuthResponseDto> LoginAsync(LoginCustomerDto input);
    Task<CustomerAuthResponseDto> RefreshTokenAsync(RefreshCustomerTokenDto input);
    Task<string> ForgotPasswordAsync(string email);
    Task ResetPasswordAsync(ResetCustomerPasswordDto input);
}