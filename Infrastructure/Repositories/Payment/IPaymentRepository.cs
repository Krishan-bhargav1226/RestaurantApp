using Domain.Entities;

namespace Infrastructure.Repositories.Payments;

public interface IPaymentRepository
{
    Task<Payment> CreateAsync(Payment payment);
    Task<List<Payment>> GetAllAsync();
    Task<Payment?> GetByIdAsync(int id);
    Task<Payment> UpdateAsync(Payment payment);
    Task DeleteAsync(Payment payment);
}