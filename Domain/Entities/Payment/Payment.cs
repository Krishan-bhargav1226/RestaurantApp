using Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Payment : BaseEntity
{
    [Required]
    public int OrderId { get; set; }

    public int? VenueId { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    public PaymentMethod PaymentMethod { get; set; }

    [Required]
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public PaymentProvider? Provider { get; set; }

    [MaxLength(100)]
    public string? ProviderOrderId { get; set; }

    [MaxLength(100)]
    public string? ProviderPaymentId { get; set; }

    [MaxLength(100)]
    public string? ProviderSignature { get; set; }

    [MaxLength(3)]
    public string Currency { get; set; } = "INR";

    public PaymentMethodType? Method { get; set; }

    public PaymentRecordStatus? RecordStatus { get; set; }

    [MaxLength(150)]
    public string? TransactionId { get; set; }

    public DateTime InitiatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? PaidAt { get; set; }

    public DateTime? FailedAt { get; set; }

    [MaxLength(500)]
    public string? FailureReason { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public string? RawResponseJson { get; set; }

    public bool IsRefunded { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? RefundAmount { get; set; }

    [MaxLength(500)]
    public string? RefundReason { get; set; }
}