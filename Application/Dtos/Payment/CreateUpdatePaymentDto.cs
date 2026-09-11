using System.ComponentModel.DataAnnotations;
using Domain.Entities.Enums;

namespace Application.Dtos.Payments;

public class CreateUpdatePaymentDto
{
    [Required]
    public int OrderId { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    public PaymentMethod PaymentMethod { get; set; }

    [Required]
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    [MaxLength(150)]
    public string? TransactionId { get; set; }

    public DateTime? PaidAt { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public bool IsRefunded { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? RefundAmount { get; set; }

    [MaxLength(500)]
    public string? RefundReason { get; set; }
}