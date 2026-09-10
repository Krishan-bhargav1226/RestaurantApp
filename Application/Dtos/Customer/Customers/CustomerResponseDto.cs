namespace Application.Dtos.Customers;

public class CustomerResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? ProfileImagePath { get; set; }
    public int LoyaltyPoints { get; set; }
    public bool IsActive { get; set; }
    public bool IsEmailVerified { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
