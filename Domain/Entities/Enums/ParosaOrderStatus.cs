namespace Domain.Entities.Enums;

public enum ParosaOrderStatus
{
    Placed = 1,
    PaymentPending = 2,
    Paid = 3,
    Accepted = 4,
    Rejected = 5,
    Preparing = 6,
    Prepared = 7,
    PickedUp = 8,
    Delivered = 9,
    ConfirmedByCustomer = 10,
    Cancelled = 11,
    Refunded = 12
}
