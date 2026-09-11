using Application.Dtos.OrderItems;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping;

public class OrderItemProfile : Profile
{
    public OrderItemProfile()
    {
        CreateMap<CreateUpdateOrderItemCrudDto, OrderItem>();
        CreateMap<OrderItem, OrderItemResponseDto>();
    }
}