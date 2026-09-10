using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<BranchProduct> BranchProducts { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<TableStatusHistory> TableStatusHistories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<PasswordResetOTP> PasswordResetOTPs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne<Category>()
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

            modelBuilder.Entity<Branch>()
                .HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Category>()
                .HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Product>()
                .HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<BranchProduct>()
                .HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Ingredient>()
                .HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<RecipeIngredient>()
                .HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Table>()
                .HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<TableStatusHistory>()
                .HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Order>()
                .HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<OrderItem>()
                .HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<User>()
                .HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Customer>()
                .HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<CustomerAddress>()
                .HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<PasswordResetOTP>()
                .HasQueryFilter(x => !x.IsDeleted);
        }
    }
}