using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Dtos.Auth.Customer;
using Domain.Entities;
using DomainCustomer = Domain.Entities.Customer;
using DomainPasswordResetOTP = Domain.Entities.PasswordResetOTP;
using Infrastructure.Repositories.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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
        if (await _authRepository.GetCustomerAsync(email) != null) throw new InvalidOperationException("Email is already registered.");
        var customer = new DomainCustomer { FullName = input.FullName.Trim(), Email = email, PasswordHash = HashPassword(input.Password), IsEmailVerified = false };
        return CreateRegistrationResponse(await _authRepository.CreateCustomerAsync(customer));
    }

    public async Task<string> GenerateRegistrationOtpAsync(string email)
    {
        var normalizedEmail = NormalizeEmail(email);
        var customer = await _authRepository.GetCustomerAsync(normalizedEmail);
        if (customer == null) throw new KeyNotFoundException("Customer not found.");
        if (customer.IsEmailVerified) throw new InvalidOperationException("Email is already verified.");
        var otp = GenerateOtp();
        await _authRepository.CreatePasswordResetOTPAsync(new DomainPasswordResetOTP { PhoneOrEmail = normalizedEmail, Purpose = RegistrationOtpPurpose, OTPHash = HashToken(otp), ExpiresAt = DateTime.UtcNow.AddMinutes(10), IsUsed = false });
        return otp;
    }

    public async Task<CustomerRegistrationResponseDto> VerifyRegistrationOtpAsync(VerifyCustomerOtpDto input)
    {
        var email = NormalizeEmail(input.Email);
        var otp = await _authRepository.GetPasswordResetOTPAsync(email, HashToken(input.OTP.Trim()), RegistrationOtpPurpose);
        if (otp == null || otp.ExpiresAt <= DateTime.UtcNow || otp.IsUsed) throw new InvalidOperationException("OTP is invalid, expired or already used.");
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
        if (customer == null || !VerifyPassword(input.Password, customer.PasswordHash)) throw new UnauthorizedAccessException("Invalid email or password.");
        if (!customer.IsEmailVerified) throw new UnauthorizedAccessException("Please verify your email before logging in.");
        var response = CreateAuthResponse(customer);
        await SaveRefreshTokenAsync(customer, response);
        return response;
    }

    public async Task<CustomerAuthResponseDto> RefreshTokenAsync(RefreshCustomerTokenDto input)
    {
        var customer = await _authRepository.GetCustomerByRefreshTokenAsync(HashToken(input.RefreshToken));
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
        var otp = GenerateOtp();
        await _authRepository.CreatePasswordResetOTPAsync(new DomainPasswordResetOTP { PhoneOrEmail = normalizedEmail, Purpose = PasswordResetOtpPurpose, OTPHash = HashToken(otp), ExpiresAt = DateTime.UtcNow.AddMinutes(10), IsUsed = false });
        return otp;
    }

    public async Task ResetPasswordAsync(ResetCustomerPasswordDto input)
    {
        var email = NormalizeEmail(input.Email);
        var otp = await _authRepository.GetPasswordResetOTPAsync(email, HashToken(input.OTP.Trim()), PasswordResetOtpPurpose);
        if (otp == null || otp.ExpiresAt <= DateTime.UtcNow || otp.IsUsed) throw new InvalidOperationException("OTP is invalid, expired or already used.");
        var customer = await _authRepository.GetCustomerAsync(email);
        if (customer == null) throw new KeyNotFoundException("Customer not found.");
        customer.PasswordHash = HashPassword(input.NewPassword); customer.RefreshTokenHash = null; customer.RefreshTokenExpiry = null; customer.UpdatedDate = DateTime.UtcNow;
        await _authRepository.UpdateCustomerAsync(customer);
        otp.IsUsed = true; otp.UpdatedDate = DateTime.UtcNow;
        await _authRepository.UpdatePasswordResetOTPAsync(otp);
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
    private static string GenerateOtp() => RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
    private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    private static string HashToken(string value) => Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(value)));
    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100000, HashAlgorithmName.SHA256, 32);
        return $"100000.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }
    private static bool VerifyPassword(string password, string storedPassword)
    {
        var parts = storedPassword.Split('.', 3);
        if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations)) return false;
        try
        {
            var salt = Convert.FromBase64String(parts[1]);
            var expectedHash = Convert.FromBase64String(parts[2]);
            var actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expectedHash.Length);
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
        catch { return false; }
    }
    private CustomerAuthResponseDto CreateAuthResponse(DomainCustomer customer)
    {
        var expiryMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"] ?? "60");
        var refreshTokenDays = Convert.ToInt32(_configuration["Jwt:RefreshTokenExpiryInDays"] ?? "7");
        var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);
        var refreshExpiresAt = DateTime.UtcNow.AddDays(refreshTokenDays);
        var keyValue = _configuration["Jwt:Key"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        if (string.IsNullOrWhiteSpace(keyValue) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience)) throw new InvalidOperationException("JWT settings are not configured correctly in appsettings.json.");
        var refreshToken = GenerateRefreshToken();
        var claims = new List<Claim> { new(JwtRegisteredClaimNames.Sub, customer.Id.ToString()), new(ClaimTypes.NameIdentifier, customer.Id.ToString()), new(ClaimTypes.Name, customer.FullName), new(ClaimTypes.Email, customer.Email), new(ClaimTypes.Role, "Customer"), new("IsCustomer", "True") };
        var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer, audience, claims, expires: expiresAt, signingCredentials: credentials);
        return new CustomerAuthResponseDto { Id = customer.Id, FullName = customer.FullName, Email = customer.Email, IsEmailVerified = customer.IsEmailVerified, Token = new JwtSecurityTokenHandler().WriteToken(token), RefreshToken = refreshToken, ExpiresAt = expiresAt, RefreshTokenExpiresAt = refreshExpiresAt };
    }
    private static CustomerRegistrationResponseDto CreateRegistrationResponse(DomainCustomer customer) => new() { Id = customer.Id, FullName = customer.FullName, Email = customer.Email, IsEmailVerified = customer.IsEmailVerified };
    private async Task SaveRefreshTokenAsync(DomainCustomer customer, CustomerAuthResponseDto response)
    {
        customer.RefreshTokenHash = HashToken(response.RefreshToken); customer.RefreshTokenExpiry = response.RefreshTokenExpiresAt; customer.UpdatedDate = DateTime.UtcNow;
        await _authRepository.UpdateCustomerAsync(customer);
    }
}
