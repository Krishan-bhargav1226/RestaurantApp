using Application.Dtos.Payments;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<CreateUpdatePaymentDto, Payment>();
        CreateMap<Payment, PaymentResponseDto>();
    }
}