using Application.Applications.Auth;
using Application.Dtos.Auth.Customer;
using Infrastructure.Repositories.Auth;
using Microsoft.Extensions.Configuration;

namespace Application.Applications.Auth.Customer;

public class CustomerAuthApplication : ICustomerAuthApplication
{
    private const string RegistrationOtpPurpose = "Registration";
    private const string PasswordResetOtpPurpose = "PasswordReset";
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;

    public CustomerAuthApplication(IAuthRepository authRepository, IConfiguration configuration)
    {
        _authRepository = authRepository;
        _configuration = configuration;
    }

    public async Task<CustomerRegistrationResponseDto> RegisterAsync(RegisterCustomerDto input)
    {
        var email = NormalizeEmail(input.Email);
        if (await _authRepository.GetCustomerAsync(email) != null)
            throw new InvalidOperationException("Email is already registered.");

        var customer = new Domain.Entities.Customer
        {
            FullName = input.FullName.Trim(), Email = email,
            PasswordHash = AuthCryptoHelper.HashPassword(input.Password), IsEmailVerified = false
        };
        var result = await _authRepository.CreateCustomerAsync(customer);
        return CreateRegistrationResponse(result);
    }

    public async Task<string> GenerateRegistrationOtpAsync(string email)
    {
        var normalizedEmail = NormalizeEmail(email);
        var customer = await _authRepository.GetCustomerAsync(normalizedEmail);
        if (customer == null) throw new KeyNotFoundException("Customer not found.");
        if (customer.IsEmailVerified) throw new InvalidOperationException("Email is already verified.");

        var otp = AuthCryptoHelper.GenerateOtp();
        await _authRepository.CreatePasswordResetOTPAsync(new Domain.Entities.PasswordResetOTP
        {
            PhoneOrEmail = normalizedEmail, Purpose = RegistrationOtpPurpose,
            OTPHash = AuthCryptoHelper.HashToken(otp), ExpiresAt = DateTime.UtcNow.AddMinutes(10), IsUsed = false
        });
        return otp;
    }

    public async Task<CustomerRegistrationResponseDto> VerifyRegistrationOtpAsync(VerifyCustomerOtpDto input)
    {
        var email = NormalizeEmail(input.Email);
        var otp = await _authRepository.GetPasswordResetOTPAsync(email, AuthCryptoHelper.HashToken(input.OTP.Trim()), RegistrationOtpPurpose);
        if (otp == null || otp.ExpiresAt <= DateTime.UtcNow || otp.IsUsed)
            throw new InvalidOperationException("OTP is invalid, expired or already used.");

        var customer = await _authRepository.GetCustomerAsync(email);
        if (customer == null) throw new KeyNotFoundException("Customer not found.");
        if (customer.IsEmailVerified) throw new InvalidOperationException("Email is already verified.");

        customer.IsEmailVerified = true; customer.UpdatedDate = DateTime.UtcNow;
        await _authRepository.UpdateCustomerAsync(customer);
        otp.IsUsed = true; otp.UpdatedDate = DateTime.UtcNow;
        await _authRepository.UpdatePasswordResetOTPAsync(otp);
        return CreateRegistrationResponse(customer);
    }

    public async Task<CustomerAuthResponseDto> LoginAsync(LoginCustomerDto input)
    {
        var email = NormalizeEmail(input.Email);
        var customer = await _authRepository.GetCustomerAsync(email);
        if (customer == null || !AuthCryptoHelper.VerifyPassword(input.Password, customer.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");
        if (!customer.IsEmailVerified)
            throw new UnauthorizedAccessException("Please verify your email before logging in.");

        var response = CreateAuthResponse(customer);
        await SaveRefreshTokenAsync(customer, response);
        return response;
    }

    public async Task<CustomerAuthResponseDto> RefreshTokenAsync(RefreshCustomerTokenDto input)
    {
        var customer = await _authRepository.GetCustomerByRefreshTokenAsync(AuthCryptoHelper.HashToken(input.RefreshToken));
        if (customer == null) throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        if (!customer.IsEmailVerified) throw new UnauthorizedAccessException("Email is not verified.");

        var response = CreateAuthResponse(customer);
        await SaveRefreshTokenAsync(customer, response);
        return response;
    }

    public async Task<string> ForgotPasswordAsync(string email)
    {
        var normalizedEmail = NormalizeEmail(email);
        var customer = await _authRepository.GetCustomerAsync(normalizedEmail);
        if (customer == null) throw new KeyNotFoundException("Customer not found.");

        var otp = AuthCryptoHelper.GenerateOtp();
        await _authRepository.CreatePasswordResetOTPAsync(new Domain.Entities.PasswordResetOTP
        {
            PhoneOrEmail = normalizedEmail, Purpose = PasswordResetOtpPurpose,
            OTPHash = AuthCryptoHelper.HashToken(otp), ExpiresAt = DateTime.UtcNow.AddMinutes(10), IsUsed = false
        });
        return otp;
    }

    public async Task ResetPasswordAsync(ResetCustomerPasswordDto input)
    {
        var email = NormalizeEmail(input.Email);
        var otp = await _authRepository.GetPasswordResetOTPAsync(email, AuthCryptoHelper.HashToken(input.OTP.Trim()), PasswordResetOtpPurpose);
        if (otp == null || otp.ExpiresAt <= DateTime.UtcNow || otp.IsUsed)
            throw new InvalidOperationException("OTP is invalid, expired or already used.");

        var customer = await _authRepository.GetCustomerAsync(email);
        if (customer == null) throw new KeyNotFoundException("Customer not found.");
        customer.PasswordHash = AuthCryptoHelper.HashPassword(input.NewPassword);
        customer.RefreshTokenHash = null; customer.RefreshTokenExpiry = null; customer.UpdatedDate = DateTime.UtcNow;
        await _authRepository.UpdateCustomerAsync(customer);
        otp.IsUsed = true; otp.UpdatedDate = DateTime.UtcNow;
        await _authRepository.UpdatePasswordResetOTPAsync(otp);
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

    private static CustomerRegistrationResponseDto CreateRegistrationResponse(Domain.Entities.Customer customer) => new()
    {
        Id = customer.Id, FullName = customer.FullName, Email = customer.Email,
        IsEmailVerified = customer.IsEmailVerified
    };

    private CustomerAuthResponseDto CreateAuthResponse(Domain.Entities.Customer customer)
    {
        var response = new CustomerAuthResponseDto
        {
            Id = customer.Id, FullName = customer.FullName, Email = customer.Email,
            IsEmailVerified = customer.IsEmailVerified
        };
        var tokens = AuthCryptoHelper.GenerateTokens(customer.Id, customer.FullName, customer.Email, "Customer", null, true, _configuration);
        response.Token = tokens.Token; response.RefreshToken = tokens.RefreshToken;
        response.ExpiresAt = tokens.ExpiresAt; response.RefreshTokenExpiresAt = tokens.RefreshTokenExpiresAt;
        return response;
    }

    private async Task SaveRefreshTokenAsync(Domain.Entities.Customer customer, CustomerAuthResponseDto response)
    {
        customer.RefreshTokenHash = AuthCryptoHelper.HashToken(response.RefreshToken);
        customer.RefreshTokenExpiry = response.RefreshTokenExpiresAt; customer.UpdatedDate = DateTime.UtcNow;
        await _authRepository.UpdateCustomerAsync(customer);
    }
}