using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Parosa;

public abstract class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly DataContext Context;
    protected readonly DbSet<T> Set;

    protected Repository(DataContext context)
    {
        Context = context;
        Set = context.Set<T>();
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        Set.Add(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual Task<List<T>> GetAllAsync()
    {
        return Set.AsNoTracking().ToListAsync();
    }

    public virtual Task<T?> GetByIdAsync(int id)
    {
        return Set.FirstOrDefaultAsync(x => x.Id == id);
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;
        Set.Update(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task DeleteAsync(T entity)
    {
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        Set.Update(entity);
        await Context.SaveChangesAsync();
    }
}