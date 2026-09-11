using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Auth.User;

public class ResendUserRegistrationOtpDto
{
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;
}