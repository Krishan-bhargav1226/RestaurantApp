using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Branches
{
    public interface IBranchRepository
    {
       public Task<Branch> CreateAsync(Branch branch);

        Task<List<Branch>> GetAllAsync();

        Task<Branch?> GetByIdAsync(int id);

        Task<Branch> UpdateAsync(Branch branch);

        Task DeleteAsync(Branch branch);

    }
}
