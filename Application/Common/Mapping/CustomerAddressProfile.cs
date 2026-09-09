using Application.Dtos.CustomerAddresses;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping;

public class CustomerAddressProfile : Profile
{
    public CustomerAddressProfile()
    {
        CreateMap<CreateUpdateCustomerAddressDto, CustomerAddress>();
        CreateMap<CustomerAddress, CustomerAddressDto>();
    }
}
