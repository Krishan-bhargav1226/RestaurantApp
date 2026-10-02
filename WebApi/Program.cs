using Application.Applications.Branches;
using Application.Applications.BranchProducts;
using Application.Applications.Categories;
using Application.Applications.CustomerAddresses;
using Application.Applications.Customers;
using Application.Applications.Ingredients;
using Application.Applications.LoyaltyRewards;
using Application.Applications.LoyaltyTransactions;
using Application.Applications.OrderItems;
using Application.Applications.Orders;
using Application.Applications.PasswordResetOTPs;
using Application.Applications.Payments;
using Application.Applications.Products;
using Application.Applications.RecipeIngredients;
using Application.Applications.StockItems;
using Application.Applications.StockTransactions;
using Application.Applications.Tables;
using Application.Applications.TableSessions;
using Application.Applications.TableStatusHistories;
using Application.Applications.Users;
using Application.Common.Mapping;
using Infrastructure;
using Infrastructure.MultiTenancy;
using Infrastructure.Repositories.Branches;
using Infrastructure.Repositories.BranchProducts;
using Infrastructure.Repositories.Categories;
using Infrastructure.Repositories.CustomerAddresses;
using Infrastructure.Repositories.Customers;
using Infrastructure.Repositories.Ingredients;
using Infrastructure.Repositories.LoyaltyRewards;
using Infrastructure.Repositories.LoyaltyTransactions;
using Infrastructure.Repositories.OrderItems;
using Infrastructure.Repositories.Orders;
using Infrastructure.Repositories.Parosa;
using Infrastructure.Repositories.PasswordResetOTPs;
using Infrastructure.Repositories.Payments;
using Infrastructure.Repositories.Products;
using Infrastructure.Repositories.RecipeIngredients;
using Infrastructure.Repositories.StockItems;
using Infrastructure.Repositories.StockTransactions;
using Infrastructure.Repositories.Tables;
using Infrastructure.Repositories.TableSessions;
using Infrastructure.Repositories.TableStatusHistories;
using Infrastructure.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RestaurantApp API",
        Version = "v1"
    });
});

builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Multi-tenancy
builder.Services.AddScoped<TenantContext>();
builder.Services.AddScoped<ITenantContext>(sp => sp.GetRequiredService<TenantContext>());

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));

// Legacy repositories
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBranchProductRepository, BranchProductRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IRecipeIngredientRepository, RecipeIngredientRepository>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<ITableStatusHistoryRepository, TableStatusHistoryRepository>();
builder.Services.AddScoped<ITableSessionRepository, TableSessionRepository>();
builder.Services.AddScoped<Infrastructure.Repositories.Orders.IOrderRepository, Infrastructure.Repositories.Orders.OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<Infrastructure.Repositories.Payments.IPaymentRepository, Infrastructure.Repositories.Payments.PaymentRepository>();
builder.Services.AddScoped<ILoyaltyTransactionRepository, LoyaltyTransactionRepository>();
builder.Services.AddScoped<ILoyaltyRewardRepository, LoyaltyRewardRepository>();
builder.Services.AddScoped<IStockItemRepository, StockItemRepository>();
builder.Services.AddScoped<IStockTransactionRepository, StockTransactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();
builder.Services.AddScoped<IPasswordResetOTPRepository, PasswordResetOTPRepository>();

// Parosa repositories
builder.Services.AddScoped<IVenueRepository, VenueRepository>();
builder.Services.AddScoped<IOutletRepository, OutletRepository>();
builder.Services.AddScoped<ISectionRepository, SectionRepository>();
builder.Services.AddScoped<ISeatTableRepository, SeatTableRepository>();
builder.Services.AddScoped<IQRCodeRepository, QRCodeRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IMenuCategoryRepository, MenuCategoryRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<Infrastructure.Repositories.Parosa.IOrderRepository, Infrastructure.Repositories.Parosa.OrderRepository>();
builder.Services.AddScoped<Infrastructure.Repositories.Parosa.IPaymentRepository, Infrastructure.Repositories.Parosa.PaymentRepository>();
builder.Services.AddScoped<IRefundRepository, RefundRepository>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<ICouponRedemptionRepository, CouponRedemptionRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IStaffShiftRepository, StaffShiftRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

// Legacy application services
builder.Services.AddScoped<IBranchApplication, BranchApplication>();
builder.Services.AddScoped<ICategoryApplication, CategoryApplication>();
builder.Services.AddScoped<IProductApplication, ProductApplication>();
builder.Services.AddScoped<IBranchProductApplication, BranchProductApplication>();
builder.Services.AddScoped<IIngredientApplication, IngredientApplication>();
builder.Services.AddScoped<IRecipeIngredientApplication, RecipeIngredientApplication>();
builder.Services.AddScoped<ITableApplication, TableApplication>();
builder.Services.AddScoped<ITableStatusHistoryApplication, TableStatusHistoryApplication>();
builder.Services.AddScoped<ITableSessionApplication, TableSessionApplication>();
builder.Services.AddScoped<IOrderApplication, OrderApplication>();
builder.Services.AddScoped<IOrderItemApplication, OrderItemApplication>();
builder.Services.AddScoped<IPaymentApplication, PaymentApplication>();
builder.Services.AddScoped<ILoyaltyTransactionApplication, LoyaltyTransactionApplication>();
builder.Services.AddScoped<ILoyaltyRewardApplication, LoyaltyRewardApplication>();
builder.Services.AddScoped<IStockItemApplication, StockItemApplication>();
builder.Services.AddScoped<IStockTransactionApplication, StockTransactionApplication>();
builder.Services.AddScoped<IUserApplication, UserApplication>();
builder.Services.AddScoped<ICustomerApplication, CustomerApplication>();
builder.Services.AddScoped<ICustomerAddressApplication, CustomerAddressApplication>();
builder.Services.AddScoped<IPasswordResetOTPApplication, PasswordResetOTPApplication>();

builder.Services.AddAutoMapper(cfg => { }, typeof(BranchProfile).Assembly);

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();

app.Run();
