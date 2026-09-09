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

namespace Application.Applications.Auth
{
    public class AuthApplication : IAuthApplication
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthApplication(
            IAuthRepository authRepository,
            IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterUserAsync(RegisterDto input)
        {
            var email = input.Email.Trim().ToLower();
            var phone = input.Phone.Trim();

            if (await _authRepository.GetUserAsync(email) != null)
            {
                throw new InvalidOperationException("Email is already registered.");
            }

            if (await _authRepository.GetUserAsync(phone) != null)
            {
                throw new InvalidOperationException("Phone is already registered.");
            }

            var user = new User
            {
                FullName = input.FullName.Trim(),
                Email = email,
                Phone = phone,
                PasswordHash = HashPassword(input.Password),
                Role = UserRole.Staff,
                IsVerified = false
            };

            var result = await _authRepository.CreateUserAsync(user);

            return CreateRegistrationResponse(result.Id, result.FullName, result.Email, result.Phone, result.Role, result.BranchId, false);
        }

        public async Task<AuthResponseDto> RegisterCustomerAsync(RegisterDto input)
        {
            var email = input.Email.Trim().ToLower();
            var phone = input.Phone.Trim();

            if (await _authRepository.GetCustomerAsync(email) != null)
            {
                throw new InvalidOperationException("Email is already registered.");
            }

            if (await _authRepository.GetCustomerAsync(phone) != null)
            {
                throw new InvalidOperationException("Phone is already registered.");
            }

            var customer = new Customer
            {
                FullName = input.FullName.Trim(),
                Email = email,
                Phone = phone,
                PasswordHash = HashPassword(input.Password),
                IsVerified = false
            };

            var result = await _authRepository.CreateCustomerAsync(customer);

            return CreateRegistrationResponse(result.Id, result.FullName, result.Email, result.Phone, null, null, true);
        }

        public async Task<string> GenerateRegistrationOtpAsync(string phoneOrEmail)
        {
            var value = phoneOrEmail.Trim().ToLower();

            var user = await _authRepository.GetUserAsync(value);
            var customer = user == null
                ? await _authRepository.GetCustomerAsync(value)
                : null;

            if (user == null && customer == null)
            {
                throw new KeyNotFoundException("User or customer not found.");
            }

            if ((user != null && user.IsVerified) || (customer != null && customer.IsVerified))
            {
                throw new InvalidOperationException("Account is already verified.");
            }

            var otp = GenerateOtp();

            var resetOtp = new PasswordResetOTP
            {
                PhoneOrEmail = value,
                OTPHash = HashToken(otp),
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };

            await _authRepository.CreatePasswordResetOTPAsync(resetOtp);

            return otp;
        }

        public async Task VerifyRegistrationOtpAsync(VerifyRegistrationOtpDto input)
        {
            var value = input.Email.Trim().ToLower();
            var otpHash = HashToken(input.OTP.Trim());

            var resetOtp = await _authRepository.GetPasswordResetOTPAsync(value, otpHash);

            if (resetOtp == null || resetOtp.ExpiresAt < DateTime.UtcNow || resetOtp.IsUsed)
            {
                throw new InvalidOperationException("OTP is invalid, expired or already used.");
            }

            var user = await _authRepository.GetUserAsync(value);

            if (user != null)
            {
                if (user.IsVerified)
                {
                    throw new InvalidOperationException("Account is already verified.");
                }

                user.IsVerified = true;
                user.UpdatedDate = DateTime.UtcNow;
                await _authRepository.UpdateUserAsync(user);
            }
            else
            {
                var customer = await _authRepository.GetCustomerAsync(value);

                if (customer == null)
                {
                    throw new KeyNotFoundException("User or customer not found.");
                }

                if (customer.IsVerified)
                {
                    throw new InvalidOperationException("Account is already verified.");
                }

                customer.IsVerified = true;
                customer.UpdatedDate = DateTime.UtcNow;
                await _authRepository.UpdateCustomerAsync(customer);
            }

            resetOtp.IsUsed = true;
            resetOtp.UpdatedDate = DateTime.UtcNow;
            await _authRepository.UpdatePasswordResetOTPAsync(resetOtp);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto input)
        {
            var loginValue = input.EmailOrPhone.Trim().ToLower();

            var user = await _authRepository.GetUserAsync(loginValue);

            if (user != null)
            {
                if (!VerifyPassword(input.Password, user.PasswordHash))
                {
                    throw new UnauthorizedAccessException("Invalid email/phone or password.");
                }

                if (!user.IsVerified)
                {
                    throw new UnauthorizedAccessException("Account is not verified. Please verify the registration OTP first.");
                }

                var response = CreateUserResponse(user);
                await SaveUserRefreshTokenAsync(user, response);
                return response;
            }

            var customer = await _authRepository.GetCustomerAsync(loginValue);

            if (customer != null)
            {
                if (!VerifyPassword(input.Password, customer.PasswordHash))
                {
                    throw new UnauthorizedAccessException("Invalid email/phone or password.");
                }

                if (!customer.IsVerified)
                {
                    throw new UnauthorizedAccessException("Account is not verified. Please verify the registration OTP first.");
                }

                var response = CreateCustomerResponse(customer);
                await SaveCustomerRefreshTokenAsync(customer, response);
                return response;
            }

            throw new UnauthorizedAccessException("Invalid email/phone or password.");
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto input)
        {
            var refreshTokenHash = HashToken(input.RefreshToken);

            var user = await _authRepository.GetUserByRefreshTokenAsync(refreshTokenHash);

            if (user != null)
            {
                if (!user.IsVerified)
                {
                    throw new UnauthorizedAccessException("Account is not verified.");
                }

                var response = CreateUserResponse(user);
                await SaveUserRefreshTokenAsync(user, response);
                return response;
            }

            var customer = await _authRepository.GetCustomerByRefreshTokenAsync(refreshTokenHash);

            if (customer != null)
            {
                if (!customer.IsVerified)
                {
                    throw new UnauthorizedAccessException("Account is not verified.");
                }

                var response = CreateCustomerResponse(customer);
                await SaveCustomerRefreshTokenAsync(customer, response);
                return response;
            }

            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }

        public async Task<string> ForgotPasswordAsync(string phoneOrEmail)
        {
            var value = phoneOrEmail.Trim().ToLower();

            var user = await _authRepository.GetUserAsync(value);
            var customer = user == null
                ? await _authRepository.GetCustomerAsync(value)
                : null;

            if (user == null && customer == null)
            {
                throw new KeyNotFoundException("User or customer not found.");
            }

            var otp = GenerateOtp();

            var resetOtp = new PasswordResetOTP
            {
                PhoneOrEmail = value,
                OTPHash = HashToken(otp),
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };

            await _authRepository.CreatePasswordResetOTPAsync(resetOtp);

            return otp;
        }

        public async Task ResetPasswordAsync(ResetPasswordDto input)
        {
            var value = input.PhoneOrEmail.Trim().ToLower();
            var otpHash = HashToken(input.OTP.Trim());

            var resetOtp = await _authRepository.GetPasswordResetOTPAsync(value, otpHash);

            if (resetOtp == null || resetOtp.ExpiresAt < DateTime.UtcNow || resetOtp.IsUsed)
            {
                throw new InvalidOperationException("OTP is invalid, expired or already used.");
            }

            var user = await _authRepository.GetUserAsync(value);

            if (user != null)
            {
                user.PasswordHash = HashPassword(input.NewPassword);
                user.RefreshTokenHash = null;
                user.RefreshTokenExpiry = null;
                user.UpdatedDate = DateTime.UtcNow;
                await _authRepository.UpdateUserAsync(user);
            }
            else
            {
                var customer = await _authRepository.GetCustomerAsync(value);

                if (customer == null)
                {
                    throw new KeyNotFoundException("User or customer not found.");
                }

                customer.PasswordHash = HashPassword(input.NewPassword);
                customer.RefreshTokenHash = null;
                customer.RefreshTokenExpiry = null;
                customer.UpdatedDate = DateTime.UtcNow;
                await _authRepository.UpdateCustomerAsync(customer);
            }

            resetOtp.IsUsed = true;
            resetOtp.UpdatedDate = DateTime.UtcNow;
            await _authRepository.UpdatePasswordResetOTPAsync(resetOtp);
        }

        private static AuthResponseDto CreateRegistrationResponse(
            int id,
            string fullName,
            string email,
            string phone,
            UserRole? role,
            int? branchId,
            bool isCustomer)
        {
            return new AuthResponseDto
            {
                Id = id,
                FullName = fullName,
                Email = email,
                Phone = phone,
                Role = role,
                BranchId = branchId,
                IsCustomer = isCustomer,
                Token = string.Empty,
                RefreshToken = string.Empty
            };
        }

        private AuthResponseDto CreateUserResponse(User user)
        {
            var response = new AuthResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                BranchId = user.BranchId,
                IsCustomer = false
            };

            AddTokens(response, user.Id, user.FullName, user.Email, user.Role.ToString(), user.BranchId, false);
            return response;
        }

        private AuthResponseDto CreateCustomerResponse(Customer customer)
        {
            var response = new AuthResponseDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                Phone = customer.Phone,
                Role = null,
                BranchId = null,
                IsCustomer = true
            };

            AddTokens(response, customer.Id, customer.FullName, customer.Email, "Customer", null, true);
            return response;
        }

        private async Task SaveUserRefreshTokenAsync(User user, AuthResponseDto response)
        {
            user.RefreshTokenHash = HashToken(response.RefreshToken);
            user.RefreshTokenExpiry = response.RefreshTokenExpiresAt;
            user.UpdatedDate = DateTime.UtcNow;
            await _authRepository.UpdateUserAsync(user);
        }

        private async Task SaveCustomerRefreshTokenAsync(Customer customer, AuthResponseDto response)
        {
            customer.RefreshTokenHash = HashToken(response.RefreshToken);
            customer.RefreshTokenExpiry = response.RefreshTokenExpiresAt;
            customer.UpdatedDate = DateTime.UtcNow;
            await _authRepository.UpdateCustomerAsync(customer);
        }

        private void AddTokens(
            AuthResponseDto response,
            int id,
            string fullName,
            string email,
            string role,
            int? branchId,
            bool isCustomer)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(jwtKey) ||
                string.IsNullOrWhiteSpace(issuer) ||
                string.IsNullOrWhiteSpace(audience))
            {
                throw new InvalidOperationException("JWT settings are not configured correctly in appsettings.json.");
            }

            var expiryMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"] ?? "60");
            var refreshTokenDays = Convert.ToInt32(_configuration["Jwt:RefreshTokenExpiryInDays"] ?? "7");
            var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);
            var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(refreshTokenDays);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, fullName),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim("IsCustomer", isCustomer.ToString())
            };

            if (branchId.HasValue)
            {
                claims.Add(new Claim("BranchId", branchId.Value.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expiresAt,
                signingCredentials: credentials);

            response.Token = new JwtSecurityTokenHandler().WriteToken(token);
            response.RefreshToken = GenerateRefreshToken();
            response.ExpiresAt = expiresAt;
            response.RefreshTokenExpiresAt = refreshTokenExpiresAt;
        }

        private static string GenerateOtp()
        {
            return RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
        }

        private static string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

        private static string HashToken(string value)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
            return Convert.ToBase64String(bytes);
        }

        private static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100000, HashAlgorithmName.SHA256, 32);
            return $"100000.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        private static bool VerifyPassword(string password, string storedPassword)
        {
            var parts = storedPassword.Split('.', 3);

            if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations))
            {
                return false;
            }

            try
            {
                var salt = Convert.FromBase64String(parts[1]);
                var expectedHash = Convert.FromBase64String(parts[2]);
                var actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expectedHash.Length);
                return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
            }
            catch
            {
                return false;
            }
        }
    }
}