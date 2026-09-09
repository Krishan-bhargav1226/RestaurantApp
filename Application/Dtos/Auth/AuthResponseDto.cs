using Domain.Entities.Enums;

namespace Application.Dtos.Auth
{
    public class AuthResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }
        public UserRole? Role { get; set; }
        public int? BranchId { get; set; }
        public bool IsCustomer { get; set; }
    }
}