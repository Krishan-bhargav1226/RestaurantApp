using System.Text;
using Application.Applications.Auth;
using Application.Applications.BranchProducts;
using Application.Applications.Branches;
using Application.Applications.Categories;
using Application.Applications.CustomerAddresses;
using Application.Applications.Customers;
using Application.Applications.Ingredients;
using Application.Applications.Orders;
using Application.Applications.Products;
using Application.Applications.RecipeIngredients;
using Application.Applications.TableStatusHistories;
using Application.Applications.Tables;
using Application.Applications.Users;
using Application.Common.Mapping;
using Infrastructure;
using Infrastructure.Repositories.Auth;
using Infrastructure.Repositories.BranchProducts;
using Infrastructure.Repositories.Branches;
using Infrastructure.Repositories.Categories;
using Infrastructure.Repositories.CustomerAddresses;
using Infrastructure.Repositories.Customers;
using Infrastructure.Repositories.Ingredients;
using Infrastructure.Repositories.Orders;
using Infrastructure.Repositories.Products;
using Infrastructure.Repositories.RecipeIngredients;
using Infrastructure.Repositories.TableStatusHistories;
using Infrastructure.Repositories.Tables;
using Infrastructure.Repositories.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using WebApi.Services;

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

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
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
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<IBranchApplication, BranchApplication>();
builder.Services.AddScoped<ICategoryApplication, CategoryApplication>();
builder.Services.AddScoped<IProductApplication, ProductApplication>();
builder.Services.AddScoped<IBranchProductApplication, BranchProductApplication>();
builder.Services.AddScoped<IIngredientApplication, IngredientApplication>();
builder.Services.AddScoped<IRecipeIngredientApplication, RecipeIngredientApplication>();
builder.Services.AddScoped<ITableApplication, TableApplication>();
builder.Services.AddScoped<ITableStatusHistoryApplication, TableStatusHistoryApplication>();
builder.Services.AddScoped<IOrderApplication, OrderApplication>();
builder.Services.AddScoped<IAuthApplication, AuthApplication>();
builder.Services.AddScoped<ICustomerAddressApplication, CustomerAddressApplication>();
builder.Services.AddScoped<IUserApplication, UserApplication>();
builder.Services.AddScoped<ICustomerApplication, CustomerApplication>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddAutoMapper(cfg => { }, typeof(BranchProfile).Assembly);

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtKey) ||
    string.IsNullOrWhiteSpace(jwtIssuer) ||
    string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new InvalidOperationException("JWT settings are not configured correctly in appsettings.json.");
}

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();