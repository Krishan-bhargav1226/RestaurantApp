using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Auth
{
    public class ResetPasswordDto
    {
        [Required]
        public string PhoneOrEmail { get; set; } = string.Empty;

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string OTP { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;
    }
}