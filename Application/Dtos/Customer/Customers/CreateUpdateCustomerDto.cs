using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Customers;

public class CreateUpdateCustomerDto
{
    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    [Phone]
    public string? Phone { get; set; }

    [MaxLength(500)]
    public string? ProfileImagePath { get; set; }

    [Range(0, int.MaxValue)]
    public int LoyaltyPoints { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(100)]
    public string? Password { get; set; }
}
