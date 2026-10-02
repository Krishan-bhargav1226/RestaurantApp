using Domain.Entities;

namespace Infrastructure.Repositories.Parosa;

public interface IRepository<T> where T : BaseEntity
{
    Task<T> CreateAsync(T entity);
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}