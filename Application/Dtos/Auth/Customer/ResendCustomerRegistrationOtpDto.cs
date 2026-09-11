using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Auth.Customer;

public class ResendCustomerRegistrationOtpDto
{
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;
}