using System.ComponentModel.DataAnnotations;
using Domain.Entities.Enums;

namespace Application.Dtos.LoyaltyTransactions;

public class CreateUpdateLoyaltyTransactionDto
{
    [Required]
    public int CustomerId { get; set; }

    [Required]
    public int BranchId { get; set; }

    public int Points { get; set; }

    [Required]
    public LoyaltyTransactionType Type { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
}