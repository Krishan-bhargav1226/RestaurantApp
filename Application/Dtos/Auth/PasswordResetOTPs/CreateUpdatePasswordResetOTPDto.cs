using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.PasswordResetOTPs;

public class CreateUpdatePasswordResetOTPDto
{
    [Required]
    [MaxLength(150)]
    public string PhoneOrEmail { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Purpose { get; set; } = string.Empty;

    [Required]
    public string OTPHash { get; set; } = string.Empty;

    [Required]
    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }
}