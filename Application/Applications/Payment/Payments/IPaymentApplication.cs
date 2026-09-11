using Application.Dtos.Payments;

namespace Application.Applications.Payments;

public interface IPaymentApplication
{
    Task<PaymentResponseDto> CreateAsync(CreateUpdatePaymentDto input);
    Task<List<PaymentResponseDto>> GetAllAsync();
    Task<PaymentResponseDto> GetByIdAsync(int id);
    Task<PaymentResponseDto> UpdateAsync(int id, CreateUpdatePaymentDto input);
    Task DeleteAsync(int id);
}