using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Dtos.Auth;
using Domain.Entities;
using Domain.Entities.Enums;
using Infrastructure.Repositories.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Applications.Auth;

public class AuthApplication : IAuthApplication
{
    private readonly IAuthRepository _repository;
    private readonly IConfiguration _configuration;

    public AuthApplication(IAuthRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterUserAsync(RegisterDto input)
    {
        if (await _repository.UserEmailOrPhoneExistsAsync(input.Email, input.Phone))
            throw new InvalidOperationException("A user with this email or phone already exists.");

        var user = new User
        {
            FullName = input.FullName.Trim(), Email = input.Email.Trim().ToLowerInvariant(), Phone = input.Phone.Trim(),
            PasswordHash = HashPassword(input.Password), Role = UserRole.Staff
        };
        await _repository.CreateUserAsync(user);
        return CreateResponse(user);
    }

    public async Task<AuthResponseDto> RegisterCustomerAsync(RegisterDto input)
    {
        if (await _repository.CustomerEmailOrPhoneExistsAsync(input.Email, input.Phone))
            throw new InvalidOperationException("A customer with this email or phone already exists.");

        var customer = new Customer
        {
            FullName = input.FullName.Trim(), Email = input.Email.Trim().ToLowerInvariant(), Phone = input.Phone.Trim(),
            PasswordHash = HashPassword(input.Password)
        };
        await _repository.CreateCustomerAsync(customer);
        return CreateResponse(customer);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto input)
    {
        var user = await _repository.GetUserAsync(input.EmailOrPhone.Trim());
        if (user != null && VerifyPassword(input.Password, user.PasswordHash)) return CreateResponse(user);

        var customer = await _repository.GetCustomerAsync(input.EmailOrPhone.Trim());
        if (customer != null && VerifyPassword(input.Password, customer.PasswordHash)) return CreateResponse(customer);

        throw new UnauthorizedAccessException("Invalid email/phone or password.");
    }

    private AuthResponseDto CreateResponse(User user)
    {
        var (token, expires) = GenerateToken(user.Id, user.FullName, user.Email, user.Role.ToString(), user.BranchId, false);
        return new AuthResponseDto { Id = user.Id, FullName = user.FullName, Email = user.Email, Phone = user.Phone, Token = token, ExpiresAt = expires, Role = user.Role, BranchId = user.BranchId, IsCustomer = false };
    }

    private AuthResponseDto CreateResponse(Customer customer)
    {
        var (token, expires) = GenerateToken(customer.Id, customer.FullName, customer.Email, "Customer", null, true);
        return new AuthResponseDto { Id = customer.Id, FullName = customer.FullName, Email = customer.Email, Phone = customer.Phone, Token = token, ExpiresAt = expires, Role = null, BranchId = null, IsCustomer = true };
    }

    private (string Token, DateTime Expires) GenerateToken(int id, string name, string email, string role, int? branchId, bool isCustomer)
    {
        var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");
        var issuer = _configuration["Jwt:Issuer"] ?? "RestaurantApp";
        var audience = _configuration["Jwt:Audience"] ?? "RestaurantApp";
        var minutes = int.TryParse(_configuration["Jwt:ExpiryMinutes"], out var m) ? m : 60;
        var expires = DateTime.UtcNow.AddMinutes(minutes);
        var claims = new List<Claim> { new(JwtRegisteredClaimNames.Sub, id.ToString()), new(ClaimTypes.Name, name), new(ClaimTypes.Email, email), new(ClaimTypes.Role, role), new("IsCustomer", isCustomer.ToString()) };
        if (branchId.HasValue) claims.Add(new Claim("BranchId", branchId.Value.ToString()));
        var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer, audience, claims, expires: expires, signingCredentials: credentials);
        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }

    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);
        return $"100000.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    private static bool VerifyPassword(string password, string stored)
    {
        var parts = stored.Split('.', 3);
        if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations)) return false;
        var salt = Convert.FromBase64String(parts[1]);
        var expected = Convert.FromBase64String(parts[2]);
        var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expected.Length);
        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }
}
