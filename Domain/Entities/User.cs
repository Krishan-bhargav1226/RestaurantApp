using System.ComponentModel.DataAnnotations;
using Domain.Entities.Enums;

namespace Domain.Entities;

public class User : BaseEntity
{
    [Required, MaxLength(150)] public string FullName { get; set; } = string.Empty;
    [Required, MaxLength(150), EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, MaxLength(20)] public string Phone { get; set; } = string.Empty;
    [Required] public string PasswordHash { get; set; } = string.Empty;
    [MaxLength(500)] public string? ProfileImagePath { get; set; }
    [Required] public UserRole Role { get; set; } = UserRole.Staff;
    public int? BranchId { get; set; }
    public string? RefreshTokenHash { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
}
