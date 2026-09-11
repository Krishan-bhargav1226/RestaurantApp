using Application.Applications.BranchProducts;
using Application.Applications.Branches;
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
using Application.Applications.TableStatusHistories;
using Application.Applications.Tables;
using Application.Applications.Users;
using Application.Common.Mapping;
using Infrastructure;
using Infrastructure.Repositories.BranchProducts;
using Infrastructure.Repositories.Branches;
using Infrastructure.Repositories.Categories;
using Infrastructure.Repositories.CustomerAddresses;
using Infrastructure.Repositories.Customers;
using Infrastructure.Repositories.Ingredients;
using Infrastructure.Repositories.LoyaltyRewards;
using Infrastructure.Repositories.LoyaltyTransactions;
using Infrastructure.Repositories.OrderItems;
using Infrastructure.Repositories.Orders;
using Infrastructure.Repositories.PasswordResetOTPs;
using Infrastructure.Repositories.Payments;
using Infrastructure.Repositories.Products;
using Infrastructure.Repositories.RecipeIngredients;
using Infrastructure.Repositories.StockItems;
using Infrastructure.Repositories.StockTransactions;
using Infrastructure.Repositories.Tables;
using Infrastructure.Repositories.TableStatusHistories;
using Infrastructure.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBranchProductRepository, BranchProductRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IRecipeIngredientRepository, RecipeIngredientRepository>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<ITableStatusHistoryRepository, TableStatusHistoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ILoyaltyTransactionRepository, LoyaltyTransactionRepository>();
builder.Services.AddScoped<ILoyaltyRewardRepository, LoyaltyRewardRepository>();
builder.Services.AddScoped<IStockItemRepository, StockItemRepository>();
builder.Services.AddScoped<IStockTransactionRepository, StockTransactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();
builder.Services.AddScoped<IPasswordResetOTPRepository, PasswordResetOTPRepository>();

builder.Services.AddScoped<IBranchApplication, BranchApplication>();
builder.Services.AddScoped<ICategoryApplication, CategoryApplication>();
builder.Services.AddScoped<IProductApplication, ProductApplication>();
builder.Services.AddScoped<IBranchProductApplication, BranchProductApplication>();
builder.Services.AddScoped<IIngredientApplication, IngredientApplication>();
builder.Services.AddScoped<IRecipeIngredientApplication, RecipeIngredientApplication>();
builder.Services.AddScoped<ITableApplication, TableApplication>();
builder.Services.AddScoped<ITableStatusHistoryApplication, TableStatusHistoryApplication>();
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
app.MapControllers();

app.Run();
