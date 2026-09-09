using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class PasswordResetOTP : BaseEntity
{
    [Required, MaxLength(150)] public string PhoneOrEmail { get; set; } = string.Empty;
    [Required] public string OTPHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
}
