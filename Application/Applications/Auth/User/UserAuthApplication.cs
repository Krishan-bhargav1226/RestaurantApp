using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Dtos.Auth.User;
using Domain.Entities;
using DomainUser = Domain.Entities.User;
using DomainPasswordResetOTP = Domain.Entities.PasswordResetOTP;
using Domain.Entities.Enums;
using Infrastructure.Repositories.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Applications.Auth.User;

public class UserAuthApplication : IUserAuthApplication
{
    private const string RegistrationOtpPurpose = "Registration";
    private const string PasswordResetOtpPurpose = "PasswordReset";
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;

    public UserAuthApplication(IAuthRepository authRepository, IConfiguration configuration)
    {
        _authRepository = authRepository;
        _configuration = configuration;
    }

    public async Task<UserRegistrationResponseDto> RegisterAsync(RegisterUserDto input)
    {
        var email = NormalizeEmail(input.Email);
        if (await _authRepository.GetUserAsync(email) != null) throw new InvalidOperationException("Email is already registered.");
        var user = new DomainUser { FullName = input.FullName.Trim(), Email = email, PasswordHash = HashPassword(input.Password), Role = input.Role, IsEmailVerified = false };
        return CreateRegistrationResponse(await _authRepository.CreateUserAsync(user));
    }

    public async Task<string> GenerateRegistrationOtpAsync(string email)
    {
        var normalizedEmail = NormalizeEmail(email);
        var user = await _authRepository.GetUserAsync(normalizedEmail);
        if (user == null) throw new KeyNotFoundException("User not found.");
        if (user.IsEmailVerified) throw new InvalidOperationException("Email is already verified.");
        var otp = GenerateOtp();
        await _authRepository.CreatePasswordResetOTPAsync(new DomainPasswordResetOTP { PhoneOrEmail = normalizedEmail, Purpose = RegistrationOtpPurpose, OTPHash = HashToken(otp), ExpiresAt = DateTime.UtcNow.AddMinutes(10), IsUsed = false });
        return otp;
    }

    public async Task<UserRegistrationResponseDto> VerifyRegistrationOtpAsync(VerifyUserOtpDto input)
    {
        var email = NormalizeEmail(input.Email);
        var otp = await _authRepository.GetPasswordResetOTPAsync(email, HashToken(input.OTP.Trim()), RegistrationOtpPurpose);
        if (otp == null || otp.ExpiresAt <= DateTime.UtcNow || otp.IsUsed) throw new InvalidOperationException("OTP is invalid, expired or already used.");
        var user = await _authRepository.GetUserAsync(email);
        if (user == null) throw new KeyNotFoundException("User not found.");
        if (user.IsEmailVerified) throw new InvalidOperationException("Email is already verified.");
        user.IsEmailVerified = true; user.UpdatedDate = DateTime.UtcNow;
        await _authRepository.UpdateUserAsync(user);
        otp.IsUsed = true; otp.UpdatedDate = DateTime.UtcNow;
        await _authRepository.UpdatePasswordResetOTPAsync(otp);
        return CreateRegistrationResponse(user);
    }

    public async Task<UserAuthResponseDto> LoginAsync(LoginUserDto input)
    {
        var email = NormalizeEmail(input.Email);
        var user = await _authRepository.GetUserAsync(email);
        if (user == null || !VerifyPassword(input.Password, user.PasswordHash)) throw new UnauthorizedAccessException("Invalid email or password.");
        if (!user.IsEmailVerified) throw new UnauthorizedAccessException("Please verify your email before logging in.");
        var response = CreateAuthResponse(user);
        await SaveRefreshTokenAsync(user, response);
        return response;
    }

    public async Task<UserAuthResponseDto> RefreshTokenAsync(RefreshUserTokenDto input)
    {
        var user = await _authRepository.GetUserByRefreshTokenAsync(HashToken(input.RefreshToken));
        if (user == null) throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        if (!user.IsEmailVerified) throw new UnauthorizedAccessException("Email is not verified.");
        var response = CreateAuthResponse(user);
        await SaveRefreshTokenAsync(user, response);
        return response;
    }

    public async Task<string> ForgotPasswordAsync(string email)
    {
        var normalizedEmail = NormalizeEmail(email);
        var user = await _authRepository.GetUserAsync(normalizedEmail);
        if (user == null) throw new KeyNotFoundException("User not found.");
        var otp = GenerateOtp();
        await _authRepository.CreatePasswordResetOTPAsync(new DomainPasswordResetOTP { PhoneOrEmail = normalizedEmail, Purpose = PasswordResetOtpPurpose, OTPHash = HashToken(otp), ExpiresAt = DateTime.UtcNow.AddMinutes(10), IsUsed = false });
        return otp;
    }

    public async Task ResetPasswordAsync(ResetUserPasswordDto input)
    {
        var email = NormalizeEmail(input.Email);
        var otp = await _authRepository.GetPasswordResetOTPAsync(email, HashToken(input.OTP.Trim()), PasswordResetOtpPurpose);
        if (otp == null || otp.ExpiresAt <= DateTime.UtcNow || otp.IsUsed) throw new InvalidOperationException("OTP is invalid, expired or already used.");
        var user = await _authRepository.GetUserAsync(email);
        if (user == null) throw new KeyNotFoundException("User not found.");
        user.PasswordHash = HashPassword(input.NewPassword); user.RefreshTokenHash = null; user.RefreshTokenExpiry = null; user.UpdatedDate = DateTime.UtcNow;
        await _authRepository.UpdateUserAsync(user);
        otp.IsUsed = true; otp.UpdatedDate = DateTime.UtcNow;
        await _authRepository.UpdatePasswordResetOTPAsync(otp);
    }

    public async Task<UserRegistrationResponseDto> AssignRoleAsync(int userId, AssignRoleDto input)
    {
        var user = await _authRepository.GetUserByIdAsync(userId);
        if (user == null) throw new KeyNotFoundException("User not found.");
        user.Role = input.Role; user.UpdatedDate = DateTime.UtcNow;
        await _authRepository.UpdateUserAsync(user);
        return CreateRegistrationResponse(user);
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
    private UserAuthResponseDto CreateAuthResponse(DomainUser user)
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
        var claims = new List<Claim> { new(JwtRegisteredClaimNames.Sub, user.Id.ToString()), new(ClaimTypes.NameIdentifier, user.Id.ToString()), new(ClaimTypes.Name, user.FullName), new(ClaimTypes.Email, user.Email), new(ClaimTypes.Role, user.Role.ToString()), new("IsCustomer", "False") };
        if (user.BranchId.HasValue) claims.Add(new Claim("BranchId", user.BranchId.Value.ToString()));
        var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer, audience, claims, expires: expiresAt, signingCredentials: credentials);
        return new UserAuthResponseDto { Id = user.Id, FullName = user.FullName, Email = user.Email, Role = user.Role, BranchId = user.BranchId, IsEmailVerified = user.IsEmailVerified, Token = new JwtSecurityTokenHandler().WriteToken(token), RefreshToken = refreshToken, ExpiresAt = expiresAt, RefreshTokenExpiresAt = refreshExpiresAt };
    }
    private static UserRegistrationResponseDto CreateRegistrationResponse(DomainUser user) => new() { Id = user.Id, FullName = user.FullName, Email = user.Email, Role = user.Role, IsEmailVerified = user.IsEmailVerified };
    private async Task SaveRefreshTokenAsync(DomainUser user, UserAuthResponseDto response)
    {
        user.RefreshTokenHash = HashToken(response.RefreshToken); user.RefreshTokenExpiry = response.RefreshTokenExpiresAt; user.UpdatedDate = DateTime.UtcNow;
        await _authRepository.UpdateUserAsync(user);
    }
}
