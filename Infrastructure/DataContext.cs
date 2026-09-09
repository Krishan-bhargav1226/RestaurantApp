using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Branch> Branches { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<BranchProduct> BranchProducts { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category -> Product
            modelBuilder.Entity<Product>()
                .HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Branch -> BranchProduct
            modelBuilder.Entity<BranchProduct>()
                .HasOne<Branch>()
                .WithMany()
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product -> BranchProduct
            modelBuilder.Entity<BranchProduct>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product -> RecipeIngredient
            modelBuilder.Entity<RecipeIngredient>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ingredient -> RecipeIngredient
            modelBuilder.Entity<RecipeIngredient>()
                .HasOne<Ingredient>()
                .WithMany()
                .HasForeignKey(x => x.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Soft Delete
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
        }
    }
}