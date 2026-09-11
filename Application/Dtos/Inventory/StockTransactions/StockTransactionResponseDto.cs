using Domain.Entities.Enums;

namespace Application.Dtos.StockTransactions;

public class StockTransactionResponseDto
{
    public int Id { get; set; }
    public int StockItemId { get; set; }
    public decimal Quantity { get; set; }
    public StockTransactionType Type { get; set; }
    public string? ReferenceType { get; set; }
    public int? ReferenceId { get; set; }
    public int? CreatedByUserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}