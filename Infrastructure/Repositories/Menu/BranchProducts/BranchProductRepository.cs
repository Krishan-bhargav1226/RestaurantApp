using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.BranchProducts
{
    public class BranchProductRepository : IBranchProductRepository
    {
        private readonly DataContext _context;

        public BranchProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<BranchProduct> CreateAsync(BranchProduct branchProduct)
        {
            _context.BranchProducts.Add(branchProduct);
            await _context.SaveChangesAsync();
            return branchProduct;
        }

        public async Task<List<BranchProduct>> GetAllAsync()
        {
            return await _context.BranchProducts
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<BranchProduct?> GetByIdAsync(int id)
        {
            return await _context.BranchProducts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BranchProduct> UpdateAsync(BranchProduct branchProduct)
        {
            _context.BranchProducts.Update(branchProduct);
            await _context.SaveChangesAsync();
            return branchProduct;
        }

        public async Task DeleteAsync(BranchProduct branchProduct)
        {
            branchProduct.IsDeleted = true;
            _context.BranchProducts.Update(branchProduct);
            await _context.SaveChangesAsync();
        }
    }
}
