using Domain.Entities;
using System.Collections.Generic;

namespace Infrastructure.Repositories.BranchProducts
{
    public interface IBranchProductRepository
    {
        Task<BranchProduct> CreateAsync(BranchProduct branchProduct);
        Task<List<BranchProduct>> GetAllAsync();
        Task<BranchProduct?> GetByIdAsync(int id);
        Task<BranchProduct> UpdateAsync(BranchProduct branchProduct);
        Task DeleteAsync(BranchProduct branchProduct);
    }
}
