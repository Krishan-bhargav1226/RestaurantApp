using Domain.Entities;
using Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Parosa;

public interface IVenueRepository : IRepository<Venue> { }
public interface IOutletRepository : IRepository<Outlet> { }
public interface ISectionRepository : IRepository<Section> { }
public interface ISeatTableRepository : IRepository<SeatTable> { }

public interface IQRCodeRepository : IRepository<QRCode>
{
    Task<QRCode?> GetByTokenAsync(string token);
}

public interface IRoleRepository : IRepository<Role> { }
public interface IMenuCategoryRepository : IRepository<MenuCategory> { }

public interface IMenuItemRepository : IRepository<MenuItem>
{
    Task<MenuItem?> GetDetailsAsync(int id);
}

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart?> GetActiveBySessionAsync(string sessionToken);
}

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetDetailsAsync(int id);
}

public interface IPaymentRepository : IRepository<Payment>
{
    Task<Payment?> GetByProviderPaymentIdAsync(string providerPaymentId);
}

public interface IRefundRepository : IRepository<Refund> { }

public interface ICouponRepository : IRepository<Coupon>
{
    Task<Coupon?> GetByCodeAsync(string code);
}

public interface ICouponRedemptionRepository : IRepository<CouponRedemption> { }
public interface INotificationRepository : IRepository<Notification> { }
public interface IStaffShiftRepository : IRepository<StaffShift> { }
public interface IAuditLogRepository : IRepository<AuditLog> { }
public interface IPlanRepository : IRepository<Plan> { }
public interface ISubscriptionRepository : IRepository<Subscription> { }

public sealed class VenueRepository : Repository<Venue>, IVenueRepository
{
    public VenueRepository(DataContext context) : base(context) { }
}

public sealed class OutletRepository : Repository<Outlet>, IOutletRepository
{
    public OutletRepository(DataContext context) : base(context) { }
}

public sealed class SectionRepository : Repository<Section>, ISectionRepository
{
    public SectionRepository(DataContext context) : base(context) { }
}

public sealed class SeatTableRepository : Repository<SeatTable>, ISeatTableRepository
{
    public SeatTableRepository(DataContext context) : base(context) { }
}

public sealed class QRCodeRepository : Repository<QRCode>, IQRCodeRepository
{
    public QRCodeRepository(DataContext context) : base(context) { }

    public Task<QRCode?> GetByTokenAsync(string token)
    {
        return Set
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x =>
                x.Token == token &&
                !x.IsDeleted &&
                x.Status == QRCodeStatus.Active);
    }
}

public sealed class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(DataContext context) : base(context) { }
}

public sealed class MenuCategoryRepository : Repository<MenuCategory>, IMenuCategoryRepository
{
    public MenuCategoryRepository(DataContext context) : base(context) { }
}

public sealed class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
{
    public MenuItemRepository(DataContext context) : base(context) { }

    public Task<MenuItem?> GetDetailsAsync(int id)
    {
        return Set
            .Include(x => x.Variants)
            .Include(x => x.AddOns)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}

public sealed class CartRepository : Repository<Cart>, ICartRepository
{
    public CartRepository(DataContext context) : base(context) { }

    public Task<Cart?> GetActiveBySessionAsync(string sessionToken)
    {
        return Set
            .Include(x => x.Items)
            .ThenInclude(x => x.AddOns)
            .FirstOrDefaultAsync(x =>
                x.SessionToken == sessionToken &&
                x.Status == CartStatus.Active &&
                x.ExpiresAt > DateTime.UtcNow);
    }
}

public sealed class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(DataContext context) : base(context) { }

    public Task<Order?> GetDetailsAsync(int id)
    {
        return Set
            .Include(x => x.Items)
            .ThenInclude(x => x.AddOns)
            .Include(x => x.StatusHistory)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}

public sealed class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public PaymentRepository(DataContext context) : base(context) { }

    public Task<Payment?> GetByProviderPaymentIdAsync(string providerPaymentId)
    {
        return Set.FirstOrDefaultAsync(x => x.ProviderPaymentId == providerPaymentId);
    }
}

public sealed class RefundRepository : Repository<Refund>, IRefundRepository
{
    public RefundRepository(DataContext context) : base(context) { }
}

public sealed class CouponRepository : Repository<Coupon>, ICouponRepository
{
    public CouponRepository(DataContext context) : base(context) { }

    public Task<Coupon?> GetByCodeAsync(string code)
    {
        return Set.FirstOrDefaultAsync(x => x.Code == code.Trim().ToUpperInvariant());
    }
}

public sealed class CouponRedemptionRepository : Repository<CouponRedemption>, ICouponRedemptionRepository
{
    public CouponRedemptionRepository(DataContext context) : base(context) { }
}

public sealed class NotificationRepository : Repository<Notification>, INotificationRepository
{
    public NotificationRepository(DataContext context) : base(context) { }
}

public sealed class StaffShiftRepository : Repository<StaffShift>, IStaffShiftRepository
{
    public StaffShiftRepository(DataContext context) : base(context) { }
}

public sealed class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(DataContext context) : base(context) { }
}

public sealed class PlanRepository : Repository<Plan>, IPlanRepository
{
    public PlanRepository(DataContext context) : base(context) { }
}

public sealed class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
{
    public SubscriptionRepository(DataContext context) : base(context) { }
}