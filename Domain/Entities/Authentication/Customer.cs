using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Customer : BaseEntity
{
    [Required, MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(150), EmailAddress]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    public int? VenueId { get; set; }

    [MaxLength(500)]
    public string? ProfileImagePath { get; set; }

    public int LoyaltyPoints { get; set; }

    public string? PasswordHash { get; set; }

    public string? RefreshTokenHash { get; set; }

    public DateTime? RefreshTokenExpiry { get; set; }

    public bool IsEmailVerified { get; set; }

    public bool IsPhoneVerified { get; set; }

    public DateTime? LastOtpVerifiedAt { get; set; }

    public ICollection<CustomerAddress> Addresses { get; set; } = new List<CustomerAddress>();
}