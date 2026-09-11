namespace Application.Dtos.PasswordResetOTPs;

public class PasswordResetOTPResponseDto
{
    public int Id { get; set; }
    public string PhoneOrEmail { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public string OTPHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}