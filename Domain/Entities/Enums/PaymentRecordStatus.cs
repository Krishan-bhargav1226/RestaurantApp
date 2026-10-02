namespace Domain.Entities.Enums;

public enum PaymentRecordStatus
{
    Created = 1,
    Pending = 2,
    Paid = 3,
    Failed = 4,
    Cancelled = 5,
    Refunded = 6
}
