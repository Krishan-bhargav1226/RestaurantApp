using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Auth
{
    public class VerifyRegistrationOtpDto
    {
        [Required]
        public string PhoneOrEmail { get; set; } = string.Empty;

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string OTP { get; set; } = string.Empty;
    }
}