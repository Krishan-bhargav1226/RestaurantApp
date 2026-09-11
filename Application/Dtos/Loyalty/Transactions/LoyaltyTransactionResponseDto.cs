using Domain.Entities.Enums;

namespace Application.Dtos.LoyaltyTransactions;

public class LoyaltyTransactionResponseDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int BranchId { get; set; }
    public int Points { get; set; }
    public LoyaltyTransactionType Type { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}