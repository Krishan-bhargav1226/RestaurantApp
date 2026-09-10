using Application.Applications.Auth;
using Application.Dtos.Auth.User;
using Domain.Entities;
using Infrastructure.Repositories.Auth;
using Microsoft.Extensions.Configuration;

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
        if (await _authRepository.GetUserAsync(email) != null)
            throw new InvalidOperationException("Email is already registered.");

        var user = new Domain.Entities.User
        {
            FullName = input.FullName.Trim(), Email = email,
            PasswordHash = AuthCryptoHelper.HashPassword(input.Password),
            Role = input.Role, IsEmailVerified = false
        };
        var result = await _authRepository.CreateUserAsync(user);
        return CreateRegistrationResponse(result);
    }

    public async Task<string> GenerateRegistrationOtpAsync(string email)
    {
        var normalizedEmail = NormalizeEmail(email);
        var user = await _authRepository.GetUserAsync(normalizedEmail);
        if (user == null) throw new KeyNotFoundException("User not found.");
        if (user.IsEmailVerified) throw new InvalidOperationException("Email is already verified.");

        var otp = AuthCryptoHelper.GenerateOtp();
        await _authRepository.CreatePasswordResetOTPAsync(new PasswordResetOTP
        {
            PhoneOrEmail = normalizedEmail, Purpose = RegistrationOtpPurpose,
            OTPHash = AuthCryptoHelper.HashToken(otp), ExpiresAt = DateTime.UtcNow.AddMinutes(10), IsUsed = false
        });
        return otp;
    }

    public async Task<UserRegistrationResponseDto> VerifyRegistrationOtpAsync(VerifyUserOtpDto input)
    {
        var email = NormalizeEmail(input.Email);
        var otp = await _authRepository.GetPasswordResetOTPAsync(email, AuthCryptoHelper.HashToken(input.OTP.Trim()), RegistrationOtpPurpose);
        if (otp == null || otp.ExpiresAt <= DateTime.UtcNow || otp.IsUsed)
            throw new InvalidOperationException("OTP is invalid, expired or already used.");

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
        if (user == null || !AuthCryptoHelper.VerifyPassword(input.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");
        if (!user.IsEmailVerified)
            throw new UnauthorizedAccessException("Please verify your email before logging in.");

        var response = CreateAuthResponse(user);
        await SaveRefreshTokenAsync(user, response);
        return response;
    }

    public async Task<UserAuthResponseDto> RefreshTokenAsync(RefreshUserTokenDto input)
    {
        var user = await _authRepository.GetUserByRefreshTokenAsync(AuthCryptoHelper.HashToken(input.RefreshToken));
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

        var otp = AuthCryptoHelper.GenerateOtp();
        await _authRepository.CreatePasswordResetOTPAsync(new PasswordResetOTP
        {
            PhoneOrEmail = normalizedEmail, Purpose = PasswordResetOtpPurpose,
            OTPHash = AuthCryptoHelper.HashToken(otp), ExpiresAt = DateTime.UtcNow.AddMinutes(10), IsUsed = false
        });
        return otp;
    }

    public async Task ResetPasswordAsync(ResetUserPasswordDto input)
    {
        var email = NormalizeEmail(input.Email);
        var otp = await _authRepository.GetPasswordResetOTPAsync(email, AuthCryptoHelper.HashToken(input.OTP.Trim()), PasswordResetOtpPurpose);
        if (otp == null || otp.ExpiresAt <= DateTime.UtcNow || otp.IsUsed)
            throw new InvalidOperationException("OTP is invalid, expired or already used.");

        var user = await _authRepository.GetUserAsync(email);
        if (user == null) throw new KeyNotFoundException("User not found.");
        user.PasswordHash = AuthCryptoHelper.HashPassword(input.NewPassword);
        user.RefreshTokenHash = null; user.RefreshTokenExpiry = null; user.UpdatedDate = DateTime.UtcNow;
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

    private static UserRegistrationResponseDto CreateRegistrationResponse(Domain.Entities.User user) => new()
    {
        Id = user.Id, FullName = user.FullName, Email = user.Email,
        Role = user.Role, IsEmailVerified = user.IsEmailVerified
    };

    private UserAuthResponseDto CreateAuthResponse(Domain.Entities.User user)
    {
        var response = new UserAuthResponseDto
        {
            Id = user.Id, FullName = user.FullName, Email = user.Email,
            Role = user.Role, BranchId = user.BranchId, IsEmailVerified = user.IsEmailVerified
        };
        var tokens = AuthCryptoHelper.GenerateTokens(user.Id, user.FullName, user.Email, user.Role.ToString(), user.BranchId, false, _configuration);
        response.Token = tokens.Token; response.RefreshToken = tokens.RefreshToken;
        response.ExpiresAt = tokens.ExpiresAt; response.RefreshTokenExpiresAt = tokens.RefreshTokenExpiresAt;
        return response;
    }

    private async Task SaveRefreshTokenAsync(Domain.Entities.User user, UserAuthResponseDto response)
    {
        user.RefreshTokenHash = AuthCryptoHelper.HashToken(response.RefreshToken);
        user.RefreshTokenExpiry = response.RefreshTokenExpiresAt; user.UpdatedDate = DateTime.UtcNow;
        await _authRepository.UpdateUserAsync(user);
    }
}