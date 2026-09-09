using Application.Dtos.OrderItems;
using Application.Dtos.Orders;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<CreateUpdateOrderDto, Order>()
            .ForMember(d => d.SubTotal, o => o.Ignore())
            .ForMember(d => d.TaxAmount, o => o.Ignore())
            .ForMember(d => d.GrandTotal, o => o.Ignore())
            .ForMember(d => d.DeliveryCharge, o => o.Ignore())
            .ForMember(d => d.Items, o => o.Ignore());
        CreateMap<Order, OrderResponseDto>();
        CreateMap<CreateUpdateOrderItemDto, OrderItem>();
        CreateMap<OrderItem, OrderItemResponseDto>();
    }
}
