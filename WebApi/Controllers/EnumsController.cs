using Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnumsController : ControllerBase
    {
        private static IEnumerable<object> GetEnumValues<T>() where T : struct, Enum
        {
            return Enum.GetValues<T>()
                .Select(e => new
                {
                    Id = Convert.ToInt32(e),
                    Name = e.ToString(),
                    DisplayName = Regex.Replace(e.ToString(), "(\\B[A-Z])", " $1")
                });
        }

        [HttpGet("user-roles")]
        public IActionResult GetUserRoles() => Ok(GetEnumValues<UserRole>());

        [HttpGet("staff-shift-statuses")]
        public IActionResult GetStaffShiftStatuses() => Ok(GetEnumValues<StaffShiftStatus>());

        [HttpGet("order-statuses")]
        public IActionResult GetOrderStatuses() => Ok(GetEnumValues<OrderStatus>());

        [HttpGet("order-types")]
        public IActionResult GetOrderTypes() => Ok(GetEnumValues<OrderType>());

        [HttpGet("table-statuses")]
        public IActionResult GetTableStatuses() => Ok(GetEnumValues<TableStatus>());

        [HttpGet("payment-methods")]
        public IActionResult GetPaymentMethods() => Ok(GetEnumValues<PaymentMethod>());

        [HttpGet("payment-providers")]
        public IActionResult GetPaymentProviders() => Ok(GetEnumValues<PaymentProvider>());

        [HttpGet("payment-statuses")]
        public IActionResult GetPaymentStatuses() => Ok(GetEnumValues<PaymentStatus>());

        [HttpGet("venue-types")]
        public IActionResult GetVenueTypes() => Ok(GetEnumValues<VenueType>());

        [HttpGet("outlet-types")]
        public IActionResult GetOutletTypes() => Ok(GetEnumValues<OutletType>());

        [HttpGet("section-types")]
        public IActionResult GetSectionTypes() => Ok(GetEnumValues<SectionType>());

        [HttpGet("qr-code-statuses")]
        public IActionResult GetQRCodeStatuses() => Ok(GetEnumValues<QRCodeStatus>());

        [HttpGet("qr-code-types")]
        public IActionResult GetQRCodeTypes() => Ok(GetEnumValues<QRCodeType>());

        [HttpGet("cart-statuses")]
        public IActionResult GetCartStatuses() => Ok(GetEnumValues<CartStatus>());

        [HttpGet("coupon-types")]
        public IActionResult GetCouponTypes() => Ok(GetEnumValues<CouponType>());

        [HttpGet("notification-types")]
        public IActionResult GetNotificationTypes() => Ok(GetEnumValues<NotificationType>());

        [HttpGet("notification-channels")]
        public IActionResult GetNotificationChannels() => Ok(GetEnumValues<NotificationChannel>());

        [HttpGet("food-types")]
        public IActionResult GetFoodTypes() => Ok(GetEnumValues<FoodType>());

        [HttpGet("menu-item-types")]
        public IActionResult GetMenuItemTypes() => Ok(GetEnumValues<MenuItemType>());

        [HttpGet("stock-transaction-types")]
        public IActionResult GetStockTransactionTypes() => Ok(GetEnumValues<StockTransactionType>());

        [HttpGet("subscription-statuses")]
        public IActionResult GetSubscriptionStatuses() => Ok(GetEnumValues<SubscriptionStatus>());

        [HttpGet("all")]
        public IActionResult GetAllEnums()
        {
            var allEnums = new Dictionary<string, object>
            {
                { nameof(UserRole), GetEnumValues<UserRole>() },
                { nameof(StaffShiftStatus), GetEnumValues<StaffShiftStatus>() },
                { nameof(OrderStatus), GetEnumValues<OrderStatus>() },
                { nameof(OrderType), GetEnumValues<OrderType>() },
                { nameof(TableStatus), GetEnumValues<TableStatus>() },
                { nameof(PaymentMethod), GetEnumValues<PaymentMethod>() },
                { nameof(PaymentProvider), GetEnumValues<PaymentProvider>() },
                { nameof(PaymentStatus), GetEnumValues<PaymentStatus>() },
                { nameof(VenueType), GetEnumValues<VenueType>() },
                { nameof(OutletType), GetEnumValues<OutletType>() },
                { nameof(SectionType), GetEnumValues<SectionType>() },
                { nameof(QRCodeStatus), GetEnumValues<QRCodeStatus>() },
                { nameof(QRCodeType), GetEnumValues<QRCodeType>() },
                { nameof(CartStatus), GetEnumValues<CartStatus>() },
                { nameof(CouponType), GetEnumValues<CouponType>() },
                { nameof(NotificationType), GetEnumValues<NotificationType>() },
                { nameof(NotificationChannel), GetEnumValues<NotificationChannel>() },
                { nameof(FoodType), GetEnumValues<FoodType>() },
                { nameof(MenuItemType), GetEnumValues<MenuItemType>() },
                { nameof(StockTransactionType), GetEnumValues<StockTransactionType>() },
                { nameof(SubscriptionStatus), GetEnumValues<SubscriptionStatus>() }
            };

            return Ok(allEnums);
        }
    }
}
