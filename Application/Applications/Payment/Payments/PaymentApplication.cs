using Application.Dtos.Payments;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Payments;

namespace Application.Applications.Payments;

public class PaymentApplication : IPaymentApplication
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public PaymentApplication(IPaymentRepository paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public async Task<PaymentResponseDto> CreateAsync(CreateUpdatePaymentDto input)
    {
        var payment = _mapper.Map<Payment>(input);
        var createdPayment = await _paymentRepository.CreateAsync(payment);

        return _mapper.Map<PaymentResponseDto>(createdPayment);
    }

    public async Task<List<PaymentResponseDto>> GetAllAsync()
    {
        var payments = await _paymentRepository.GetAllAsync();

        return _mapper.Map<List<PaymentResponseDto>>(payments);
    }

    public async Task<PaymentResponseDto> GetByIdAsync(int id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);

        if (payment == null)
            throw new KeyNotFoundException("Payment not found.");

        return _mapper.Map<PaymentResponseDto>(payment);
    }

    public async Task<PaymentResponseDto> UpdateAsync(int id, CreateUpdatePaymentDto input)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);

        if (payment == null)
            throw new KeyNotFoundException("Payment not found.");

        _mapper.Map(input, payment);
        payment.UpdatedDate = DateTime.UtcNow;

        var updatedPayment = await _paymentRepository.UpdateAsync(payment);

        return _mapper.Map<PaymentResponseDto>(updatedPayment);
    }

    public async Task DeleteAsync(int id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);

        if (payment == null)
            throw new KeyNotFoundException("Payment not found.");

        await _paymentRepository.DeleteAsync(payment);
    }
}