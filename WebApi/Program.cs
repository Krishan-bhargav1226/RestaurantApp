using Application.Applications.Branches;
using Application.Applications.Categories;
using Application.Applications.Products;
using Application.Common.Mapping;
using Infrastructure;
using Infrastructure.Repositories.Branches;
using Infrastructure.Repositories.Categories;
using Infrastructure.Repositories.Products;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Database
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));

// ─── Repositories ─────────────────────────────────────────────────────────
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// ─── Application Services ─────────────────────────────────────────────────
builder.Services.AddScoped<IBranchApplication, BranchApplication>();
builder.Services.AddScoped<ICategoryApplication, CategoryApplication>();
builder.Services.AddScoped<IProductApplication, ProductApplication>();


// ─── AutoMapper ───────────────────────────────────────────────────────────
builder.Services.AddAutoMapper(cfg =>
{
}, typeof(BranchProfile).Assembly);

var app = builder.Build();

app.UseExceptionHandler("/error");
// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
