using Domain.Entities;
using Infrastructure.Repositories.Branches;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Branches;

public class BranchRepository : IBranchRepository
{
    private readonly DataContext _context;

    public BranchRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Branch> CreateAsync(Branch branch)
    {
        _context.Branches.Add(branch);
        await _context.SaveChangesAsync();
        return branch;
    }

    public async Task<List<Branch>> GetAllAsync()
    {
        return await _context.Branches
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Branch?> GetByIdAsync(int id)
    {
        return await _context.Branches
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Branch> UpdateAsync(Branch branch)
    {
        _context.Branches.Update(branch);
        await _context.SaveChangesAsync();
        return branch;
    }

    public async Task DeleteAsync(Branch branch)
    {
        // Soft delete: flag the row instead of removing it. The global query
        // filter hides it from every subsequent read.
        branch.IsDeleted = true;
        _context.Branches.Update(branch);
        await _context.SaveChangesAsync();
    }
}
