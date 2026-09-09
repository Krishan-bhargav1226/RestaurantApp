using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(Product product)
        {
            product.IsDeleted = true;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
