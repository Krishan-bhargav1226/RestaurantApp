using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Auth
{
    public class ForgotPasswordDto
    {
        [Required]
        public string PhoneOrEmail { get; set; } = string.Empty;
    }
}