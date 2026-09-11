using Application.Dtos.OrderItems;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.OrderItems;

namespace Application.Applications.OrderItems;

public class OrderItemApplication : IOrderItemApplication
{
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IMapper _mapper;

    public OrderItemApplication(IOrderItemRepository orderItemRepository, IMapper mapper)
    {
        _orderItemRepository = orderItemRepository;
        _mapper = mapper;
    }

    public async Task<OrderItemResponseDto> CreateAsync(CreateUpdateOrderItemCrudDto input)
    {
        var orderItem = _mapper.Map<OrderItem>(input);

        var createdOrderItem = await _orderItemRepository.CreateAsync(orderItem);

        return _mapper.Map<OrderItemResponseDto>(createdOrderItem);
    }

    public async Task<List<OrderItemResponseDto>> GetAllAsync()
    {
        var orderItems = await _orderItemRepository.GetAllAsync();

        return _mapper.Map<List<OrderItemResponseDto>>(orderItems);
    }

    public async Task<OrderItemResponseDto> GetByIdAsync(int id)
    {
        var orderItem = await _orderItemRepository.GetByIdAsync(id);

        if (orderItem == null)
            throw new KeyNotFoundException("Order item not found.");

        return _mapper.Map<OrderItemResponseDto>(orderItem);
    }

    public async Task<OrderItemResponseDto> UpdateAsync(int id, CreateUpdateOrderItemCrudDto input)
    {
        var orderItem = await _orderItemRepository.GetByIdAsync(id);

        if (orderItem == null)
            throw new KeyNotFoundException("Order item not found.");

        _mapper.Map(input, orderItem);
        orderItem.UpdatedDate = DateTime.UtcNow;

        var updatedOrderItem = await _orderItemRepository.UpdateAsync(orderItem);

        return _mapper.Map<OrderItemResponseDto>(updatedOrderItem);
    }

    public async Task DeleteAsync(int id)
    {
        var orderItem = await _orderItemRepository.GetByIdAsync(id);

        if (orderItem == null)
            throw new KeyNotFoundException("Order item not found.");

        await _orderItemRepository.DeleteAsync(orderItem);
    }
}