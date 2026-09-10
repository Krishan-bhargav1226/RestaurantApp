using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Dtos.Auth.Customer;
using Application.Dtos.Auth.User;
using Domain.Entities;
using Domain.Entities.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Applications.Auth;

internal static class AuthCryptoHelper
{
    public static string GenerateOtp() => RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

    public static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    public static string HashToken(string value) =>
        Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(value)));

    public static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100000, HashAlgorithmName.SHA256, 32);
        return $"100000.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public static bool VerifyPassword(string password, string storedPassword)
    {
        var parts = storedPassword.Split('.', 3);
        if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations))
            return false;

        try
        {
            var salt = Convert.FromBase64String(parts[1]);
            var expectedHash = Convert.FromBase64String(parts[2]);
            var actualHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                expectedHash.Length);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
        catch
        {
            return false;
        }
    }

    public static void AddUserTokens(UserAuthResponseDto response, User user, IConfiguration configuration)
    {
        AddTokens(
            response,
            user.Id,
            user.FullName,
            user.Email,
            user.Role.ToString(),
            user.BranchId,
            false,
            configuration);
    }

    public static void AddCustomerTokens(CustomerAuthResponseDto response, Customer customer, IConfiguration configuration)
    {
        AddTokens(
            response,
            customer.Id,
            customer.FullName,
            customer.Email,
            "Customer",
            null,
            true,
            configuration);
    }

    private static void AddTokens(
        UserAuthResponseDto response,
        int id,
        string fullName,
        string email,
        string role,
        int? branchId,
        bool isCustomer,
        IConfiguration configuration)
    {
        var tokenData = CreateTokenData(id, fullName, email, role, branchId, isCustomer, configuration);
        response.Token = tokenData.Token;
        response.RefreshToken = tokenData.RefreshToken;
        response.ExpiresAt = tokenData.ExpiresAt;
        response.RefreshTokenExpiresAt = tokenData.RefreshTokenExpiresAt;
    }

    private static void AddTokens(
        CustomerAuthResponseDto response,
        int id,
        string fullName,
        string email,
        string role,
        int? branchId,
        bool isCustomer,
        IConfiguration configuration)
    {
        var tokenData = CreateTokenData(id, fullName, email, role, branchId, isCustomer, configuration);
        response.Token = tokenData.Token;
        response.RefreshToken = tokenData.RefreshToken;
        response.ExpiresAt = tokenData.ExpiresAt;
        response.RefreshTokenExpiresAt = tokenData.RefreshTokenExpiresAt;
    }

    private static TokenData CreateTokenData(
        int id,
        string fullName,
        string email,
        string role,
        int? branchId,
        bool isCustomer,
        IConfiguration configuration)
    {
        var jwtKey = configuration["Jwt:Key"];
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];

        if (string.IsNullOrWhiteSpace(jwtKey) ||
            string.IsNullOrWhiteSpace(issuer) ||
            string.IsNullOrWhiteSpace(audience))
        {
            throw new InvalidOperationException("JWT settings are not configured correctly in appsettings.json.");
        }

        var expiryMinutes = Convert.ToInt32(configuration["Jwt:ExpiryInMinutes"] ?? "60");
        var refreshTokenDays = Convert.ToInt32(configuration["Jwt:RefreshTokenExpiryInDays"] ?? "7");
        var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);
        var refreshExpiresAt = DateTime.UtcNow.AddDays(refreshTokenDays);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, id.ToString()),
            new(ClaimTypes.NameIdentifier, id.ToString()),
            new(ClaimTypes.Name, fullName),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, role),
            new("IsCustomer", isCustomer.ToString())
        };

        if (branchId.HasValue)
            claims.Add(new Claim("BranchId", branchId.Value.ToString()));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new TokenData(
            new JwtSecurityTokenHandler().WriteToken(token),
            GenerateRefreshToken(),
            expiresAt,
            refreshExpiresAt);
    }

    private sealed record TokenData(
        string Token,
        string RefreshToken,
        DateTime ExpiresAt,
        DateTime RefreshTokenExpiresAt);
}