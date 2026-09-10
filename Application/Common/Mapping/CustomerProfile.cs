using Application.Dtos.Customers;
using Application.DTOs.Customers;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<CreateUpdateCustomerDto, Customer>();
        CreateMap<Customer, CustomerResponseDto>();
    }
}
