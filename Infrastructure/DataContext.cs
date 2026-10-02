using Domain.Entities;
using Infrastructure.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DataContext : DbContext
{
    private readonly ITenantContext _tenantContext;

    public DataContext(
        DbContextOptions<DataContext> options,
        ITenantContext? tenantContext = null)
        : base(options)
    {
        _tenantContext = tenantContext ?? new TenantContext();
    }

    public DbSet<Branch> Branches { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<BranchProduct> BranchProducts { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<TableSession> TableSessions { get; set; }
    public DbSet<TableStatusHistory> TableStatusHistories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerAddress> CustomerAddresses { get; set; }
    public DbSet<PasswordResetOTP> PasswordResetOTPs { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<LoyaltyTransaction> LoyaltyTransactions { get; set; }
    public DbSet<LoyaltyReward> LoyaltyRewards { get; set; }
    public DbSet<StockItem> StockItems { get; set; }
    public DbSet<StockTransaction> StockTransactions { get; set; }

    public DbSet<Venue> Venues { get; set; }
    public DbSet<Outlet> Outlets { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<SeatTable> SeatTables { get; set; }
    public DbSet<QRCode> QRCodes { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<MenuCategory> MenuCategories { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<ItemVariant> ItemVariants { get; set; }
    public DbSet<AddOn> AddOns { get; set; }
    public DbSet<MenuItemAddOn> MenuItemAddOns { get; set; }
    public DbSet<Combo> Combos { get; set; }
    public DbSet<ComboItem> ComboItems { get; set; }
    public DbSet<OutletMenuItem> OutletMenuItems { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<CartItemAddOn> CartItemAddOns { get; set; }
    public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
    public DbSet<OrderItemAddOn> OrderItemAddOns { get; set; }
    public DbSet<Refund> Refunds { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<CouponRedemption> CouponRedemptions { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<StaffShift> StaffShifts { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureLegacyModel(modelBuilder);
        ConfigureParosaModel(modelBuilder);
        ConfigureQueryFilters(modelBuilder);
    }

    private static void ConfigureLegacyModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BranchProduct>()
            .HasOne<Branch>()
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BranchProduct>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne<Ingredient>()
            .WithMany()
            .HasForeignKey(x => x.IngredientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Table>()
            .HasOne<Branch>()
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Table>()
            .HasIndex(x => new { x.BranchId, x.TableNumber })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<TableStatusHistory>()
            .HasOne<Table>()
            .WithMany()
            .HasForeignKey(x => x.TableId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TableStatusHistory>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TableSession>()
            .HasOne<Table>()
            .WithMany()
            .HasForeignKey(x => x.TableId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TableSession>()
            .HasOne<Customer>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne<Branch>()
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne<Table>()
            .WithMany()
            .HasForeignKey(x => x.TableId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne<Customer>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne<CustomerAddress>()
            .WithMany()
            .HasForeignKey(x => x.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderItem>()
            .HasOne<Order>()
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .HasOne<Order>()
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LoyaltyTransaction>()
            .HasOne<Customer>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LoyaltyTransaction>()
            .HasOne<Branch>()
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LoyaltyReward>()
            .HasOne<Branch>()
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockItem>()
            .HasOne<Branch>()
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockTransaction>()
            .HasOne<StockItem>()
            .WithMany()
            .HasForeignKey(x => x.StockItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockTransaction>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasOne<Branch>()
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CustomerAddress>()
            .HasOne<Customer>()
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<User>()
            .HasIndex(x => x.Phone)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<Customer>()
            .HasIndex(x => x.Email)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<Customer>()
            .HasIndex(x => x.Phone)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<CustomerAddress>()
            .HasIndex(x => new { x.CustomerId, x.Label })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }

    private static void ConfigureParosaModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Venue>()
            .HasIndex(x => x.Code)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<Venue>()
            .HasIndex(x => x.Slug)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<Role>()
            .HasIndex(x => x.Code)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<Outlet>()
            .HasIndex(x => new { x.VenueId, x.Code })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<Section>()
            .HasIndex(x => new { x.OutletId, x.Code })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<SeatTable>()
            .HasIndex(x => new { x.SectionId, x.Code })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<QRCode>()
            .HasIndex(x => x.Token)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<QRCode>()
            .HasIndex(x => new { x.OutletId, x.Code })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<MenuCategory>()
            .HasIndex(x => new { x.VenueId, x.Name })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<MenuItem>()
            .HasIndex(x => new { x.VenueId, x.CategoryId, x.Name })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<ItemVariant>()
            .HasIndex(x => new { x.MenuItemId, x.Name })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<AddOn>()
            .HasIndex(x => new { x.VenueId, x.Name })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<MenuItemAddOn>()
            .HasIndex(x => new { x.MenuItemId, x.AddOnId })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<Combo>()
            .HasIndex(x => new { x.VenueId, x.Name })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<ComboItem>()
            .HasIndex(x => new { x.ComboId, x.MenuItemId })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<OutletMenuItem>()
            .HasIndex(x => new { x.OutletId, x.MenuItemId })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<Cart>()
            .HasIndex(x => new { x.VenueId, x.SessionToken })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<Order>()
            .HasIndex(x => new { x.VenueId, x.OrderNumber })
            .IsUnique()
            .HasFilter("[OrderNumber] IS NOT NULL AND [IsDeleted] = 0");

        modelBuilder.Entity<Order>()
            .HasIndex(x => new { x.VenueId, x.WorkflowStatus, x.PlacedAt });

        modelBuilder.Entity<Payment>()
            .HasIndex(x => x.ProviderPaymentId)
            .IsUnique()
            .HasFilter("[ProviderPaymentId] IS NOT NULL AND [IsDeleted] = 0");

        modelBuilder.Entity<Payment>()
            .HasIndex(x => x.ProviderOrderId)
            .HasFilter("[ProviderOrderId] IS NOT NULL AND [IsDeleted] = 0");

        modelBuilder.Entity<Refund>()
            .HasIndex(x => new { x.VenueId, x.OrderId });

        modelBuilder.Entity<Coupon>()
            .HasIndex(x => new { x.VenueId, x.Code })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<CouponRedemption>()
            .HasIndex(x => new { x.CouponId, x.CustomerId, x.OrderId })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        modelBuilder.Entity<Notification>()
            .HasIndex(x => new { x.VenueId, x.IsRead, x.CreatedDate });

        modelBuilder.Entity<StaffShift>()
            .HasIndex(x => new { x.VenueId, x.UserId, x.StartAt });

        modelBuilder.Entity<AuditLog>()
            .HasIndex(x => new { x.VenueId, x.CreatedDate });

        modelBuilder.Entity<Subscription>()
            .HasIndex(x => new { x.VenueId, x.Status });

        ConfigureMoney(modelBuilder);

        modelBuilder.Entity<Outlet>()
            .HasOne<Venue>()
            .WithMany()
            .HasForeignKey(x => x.VenueId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Section>()
            .HasOne<Outlet>()
            .WithMany()
            .HasForeignKey(x => x.OutletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SeatTable>()
            .HasOne<Section>()
            .WithMany()
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SeatTable>()
            .HasOne<Outlet>()
            .WithMany()
            .HasForeignKey(x => x.OutletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<QRCode>()
            .HasOne<Outlet>()
            .WithMany()
            .HasForeignKey(x => x.OutletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<QRCode>()
            .HasOne<SeatTable>()
            .WithMany()
            .HasForeignKey(x => x.SeatTableId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasOne<Role>()
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasOne<Venue>()
            .WithMany()
            .HasForeignKey(x => x.VenueId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasOne<Outlet>()
            .WithMany()
            .HasForeignKey(x => x.OutletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Customer>()
            .HasOne<Venue>()
            .WithMany()
            .HasForeignKey(x => x.VenueId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MenuItem>()
            .HasOne<MenuCategory>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ItemVariant>()
            .HasOne<MenuItem>()
            .WithMany(x => x.Variants)
            .HasForeignKey(x => x.MenuItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MenuItemAddOn>()
            .HasOne<MenuItem>()
            .WithMany(x => x.AddOns)
            .HasForeignKey(x => x.MenuItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MenuItemAddOn>()
            .HasOne<AddOn>()
            .WithMany()
            .HasForeignKey(x => x.AddOnId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ComboItem>()
            .HasOne<Combo>()
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.ComboId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ComboItem>()
            .HasOne<MenuItem>()
            .WithMany()
            .HasForeignKey(x => x.MenuItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OutletMenuItem>()
            .HasOne<Outlet>()
            .WithMany()
            .HasForeignKey(x => x.OutletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OutletMenuItem>()
            .HasOne<MenuItem>()
            .WithMany()
            .HasForeignKey(x => x.MenuItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cart>()
            .HasOne<Outlet>()
            .WithMany()
            .HasForeignKey(x => x.OutletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cart>()
            .HasOne<SeatTable>()
            .WithMany()
            .HasForeignKey(x => x.SeatTableId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cart>()
            .HasOne<Customer>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CartItem>()
            .HasOne<Cart>()
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CartItem>()
            .HasOne<MenuItem>()
            .WithMany()
            .HasForeignKey(x => x.MenuItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CartItem>()
            .HasOne<ItemVariant>()
            .WithMany()
            .HasForeignKey(x => x.VariantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CartItemAddOn>()
            .HasOne<CartItem>()
            .WithMany(x => x.AddOns)
            .HasForeignKey(x => x.CartItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CartItemAddOn>()
            .HasOne<AddOn>()
            .WithMany()
            .HasForeignKey(x => x.AddOnId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderStatusHistory>()
            .HasOne<Order>()
            .WithMany(x => x.StatusHistory)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderStatusHistory>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.ChangedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderItemAddOn>()
            .HasOne<OrderItem>()
            .WithMany(x => x.AddOns)
            .HasForeignKey(x => x.OrderItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItemAddOn>()
            .HasOne<AddOn>()
            .WithMany()
            .HasForeignKey(x => x.AddOnId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Refund>()
            .HasOne<Payment>()
            .WithMany()
            .HasForeignKey(x => x.PaymentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Refund>()
            .HasOne<Order>()
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CouponRedemption>()
            .HasOne<Coupon>()
            .WithMany()
            .HasForeignKey(x => x.CouponId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CouponRedemption>()
            .HasOne<Customer>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CouponRedemption>()
            .HasOne<Order>()
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Notification>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Notification>()
            .HasOne<Customer>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StaffShift>()
            .HasOne<Outlet>()
            .WithMany()
            .HasForeignKey(x => x.OutletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StaffShift>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AuditLog>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Subscription>()
            .HasOne<Venue>()
            .WithMany()
            .HasForeignKey(x => x.VenueId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Subscription>()
            .HasOne<Plan>()
            .WithMany()
            .HasForeignKey(x => x.PlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureMoney(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Venue>().Property(x => x.DefaultTaxPercentage).HasPrecision(5, 2);
        modelBuilder.Entity<MenuItem>().Property(x => x.BasePrice).HasPrecision(18, 2);
        modelBuilder.Entity<MenuItem>().Property(x => x.TaxPercentage).HasPrecision(5, 2);
        modelBuilder.Entity<ItemVariant>().Property(x => x.Price).HasPrecision(18, 2);
        modelBuilder.Entity<AddOn>().Property(x => x.Price).HasPrecision(18, 2);
        modelBuilder.Entity<Combo>().Property(x => x.Price).HasPrecision(18, 2);
        modelBuilder.Entity<OutletMenuItem>().Property(x => x.PriceOverride).HasPrecision(18, 2);
        modelBuilder.Entity<Cart>().Property(x => x.SubTotal).HasPrecision(18, 2);
        modelBuilder.Entity<Cart>().Property(x => x.TaxAmount).HasPrecision(18, 2);
        modelBuilder.Entity<Cart>().Property(x => x.GrandTotal).HasPrecision(18, 2);
        modelBuilder.Entity<CartItem>().Property(x => x.UnitPrice).HasPrecision(18, 2);
        modelBuilder.Entity<CartItemAddOn>().Property(x => x.UnitPrice).HasPrecision(18, 2);
        modelBuilder.Entity<Order>().Property(x => x.SubTotal).HasPrecision(18, 2);
        modelBuilder.Entity<Order>().Property(x => x.DiscountAmount).HasPrecision(18, 2);
        modelBuilder.Entity<Order>().Property(x => x.TaxAmount).HasPrecision(18, 2);
        modelBuilder.Entity<Order>().Property(x => x.DeliveryCharge).HasPrecision(18, 2);
        modelBuilder.Entity<Order>().Property(x => x.GrandTotal).HasPrecision(18, 2);
        modelBuilder.Entity<Order>().Property(x => x.TipAmount).HasPrecision(18, 2);
        modelBuilder.Entity<OrderItem>().Property(x => x.UnitPrice).HasPrecision(18, 2);
        modelBuilder.Entity<OrderItem>().Property(x => x.DiscountAmount).HasPrecision(18, 2);
        modelBuilder.Entity<OrderItem>().Property(x => x.TaxAmount).HasPrecision(18, 2);
        modelBuilder.Entity<OrderItem>().Property(x => x.TotalPrice).HasPrecision(18, 2);
        modelBuilder.Entity<OrderItemAddOn>().Property(x => x.UnitPrice).HasPrecision(18, 2);
        modelBuilder.Entity<OrderItemAddOn>().Property(x => x.TotalAmount).HasPrecision(18, 2);
        modelBuilder.Entity<Payment>().Property(x => x.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<Payment>().Property(x => x.RefundAmount).HasPrecision(18, 2);
        modelBuilder.Entity<Refund>().Property(x => x.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<Coupon>().Property(x => x.Value).HasPrecision(18, 2);
        modelBuilder.Entity<Coupon>().Property(x => x.MinimumOrderAmount).HasPrecision(18, 2);
        modelBuilder.Entity<Coupon>().Property(x => x.MaximumDiscountAmount).HasPrecision(18, 2);
        modelBuilder.Entity<CouponRedemption>().Property(x => x.DiscountAmount).HasPrecision(18, 2);
        modelBuilder.Entity<Plan>().Property(x => x.MonthlyPrice).HasPrecision(18, 2);
        modelBuilder.Entity<Plan>().Property(x => x.AnnualPrice).HasPrecision(18, 2);
        modelBuilder.Entity<SeatTable>().Property(x => x.PositionX).HasPrecision(9, 4);
        modelBuilder.Entity<SeatTable>().Property(x => x.PositionY).HasPrecision(9, 4);
    }

    private void ConfigureQueryFilters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Product>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<BranchProduct>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Ingredient>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<RecipeIngredient>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Table>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<TableStatusHistory>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<TableSession>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Order>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<OrderItem>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Customer>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<CustomerAddress>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<PasswordResetOTP>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Payment>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<LoyaltyTransaction>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<LoyaltyReward>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<StockItem>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<StockTransaction>().HasQueryFilter(x => !x.IsDeleted);

        modelBuilder.Entity<Venue>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Role>().HasQueryFilter(x => !x.IsDeleted);

        modelBuilder.Entity<Outlet>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<Section>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<SeatTable>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<QRCode>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<MenuCategory>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<MenuItem>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<ItemVariant>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<AddOn>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<MenuItemAddOn>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<Combo>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<ComboItem>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<OutletMenuItem>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<Cart>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<CartItem>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<CartItemAddOn>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<OrderStatusHistory>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<OrderItemAddOn>().HasQueryFilter(x => !x.IsDeleted);

        modelBuilder.Entity<Refund>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<Coupon>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<CouponRedemption>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<Notification>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<StaffShift>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<AuditLog>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<Subscription>().HasQueryFilter(x =>
            !x.IsDeleted &&
            (_tenantContext.IsPlatformAdmin || x.VenueId == _tenantContext.VenueId));

        modelBuilder.Entity<Plan>().HasQueryFilter(x => !x.IsDeleted);
    }
}
