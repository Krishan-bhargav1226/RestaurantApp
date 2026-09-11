using System.ComponentModel.DataAnnotations;
using Domain.Entities.Enums;

namespace Domain.Entities;

public class StockTransaction : BaseEntity
{
    [Required]
    public int StockItemId { get; set; }

    public decimal Quantity { get; set; }

    [Required]
    public StockTransactionType Type { get; set; }

    [MaxLength(50)]
    public string? ReferenceType { get; set; }

    public int? ReferenceId { get; set; }

    public int? CreatedByUserId { get; set; }
}