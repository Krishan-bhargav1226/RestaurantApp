namespace Domain.Entities.Enums;

public enum VenueType
{
    Cinema = 1,
    Restaurant = 2,
    Bar = 3,
    Cafe = 4,
    Hotel = 5,
    Hospital = 6,
    University = 7,
    Stadium = 8,
    Corporate = 9,
    Other = 10
}

public enum OutletType
{
    FoodCounter = 1,
    Restaurant = 2,
    Bar = 3,
    Cafe = 4,
    Concession = 5,
    Kiosk = 6,
    Other = 7
}

public enum SectionType
{
    Screen = 1,
    Hall = 2,
    DiningArea = 3,
    Floor = 4,
    Zone = 5,
    Other = 6
}

public enum LocationUnitType
{
    Seat = 1,
    Table = 2,
    Booth = 3,
    Desk = 4,
    Bed = 5,
    Other = 6
}

public enum QRCodeType
{
    SeatTable = 1,
    Outlet = 2,
    Menu = 3
}

public enum QRCodeStatus
{
    Active = 1,
    Revoked = 2
}

public enum MenuItemType
{
    Food = 1,
    Beverage = 2,
    Combo = 3,
    Other = 4
}

public enum FoodType
{
    Veg = 1,
    NonVeg = 2,
    Egg = 3,
    Vegan = 4,
    Other = 5
}

public enum CartStatus
{
    Active = 1,
    Converted = 2,
    Abandoned = 3,
    Expired = 4
}

public enum ParosaOrderType
{
    SeatDelivery = 1,
    TableDelivery = 2,
    Pickup = 3
}

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

public enum OrderStatusChangeSource
{
    Customer = 1,
    Reception = 2,
    Kitchen = 3,
    Runner = 4,
    System = 5
}

public enum PaymentProvider
{
    Razorpay = 1,
    Cashfree = 2,
    Manual = 3
}

public enum PaymentMethodType
{
    UPI = 1,
    Card = 2,
    Wallet = 3,
    NetBanking = 4,
    Cash = 5,
    Other = 6
}

public enum PaymentRecordStatus
{
    Created = 1,
    Pending = 2,
    Paid = 3,
    Failed = 4,
    Cancelled = 5,
    Refunded = 6
}

public enum RefundStatus
{
    Pending = 1,
    Processed = 2,
    Failed = 3,
    Cancelled = 4
}

public enum CouponType
{
    Percentage = 1,
    Fixed = 2
}

public enum NotificationType
{
    Order = 1,
    Payment = 2,
    Promotion = 3,
    System = 4
}

public enum NotificationChannel
{
    InApp = 1,
    Push = 2
}

public enum StaffShiftStatus
{
    Scheduled = 1,
    Active = 2,
    Completed = 3,
    Cancelled = 4
}

public enum SubscriptionStatus
{
    Trial = 1,
    Active = 2,
    PastDue = 3,
    Cancelled = 4,
    Expired = 5
}