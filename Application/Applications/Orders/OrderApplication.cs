using Application.Dtos.OrderItems;
using Application.Dtos.Orders;
using AutoMapper;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Repositories.Branches;
using Infrastructure.Repositories.Orders;
using Infrastructure.Repositories.Tables;
using Microsoft.EntityFrameworkCore;

namespace Application.Applications.Orders;

public class OrderApplication : IOrderApplication
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ITableRepository _tableRepository;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public OrderApplication(IOrderRepository orderRepository, IBranchRepository branchRepository, ITableRepository tableRepository, DataContext context, IMapper mapper)
    {
        _orderRepository = orderRepository; _branchRepository = branchRepository; _tableRepository = tableRepository; _context = context; _mapper = mapper;
    }

    public async Task<OrderResponseDto> CreateAsync(CreateUpdateOrderDto input)
    {
        var branch = await ValidateInputAsync(input);
        var order = _mapper.Map<Order>(input);
        order.OrderDate = DateTime.UtcNow;
        ApplyTotals(order, input, branch.TaxPercentage);
        foreach (var item in input.Items) order.Items.Add(MapItem(item));
        return _mapper.Map<OrderResponseDto>(await _orderRepository.CreateAsync(order));
    }
    public async Task<List<OrderResponseDto>> GetAllAsync() => _mapper.Map<List<OrderResponseDto>>(await _orderRepository.GetAllAsync());
    public async Task<OrderResponseDto> GetByIdAsync(int id) => _mapper.Map<OrderResponseDto>(await _orderRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Order not found."));
    public async Task<OrderResponseDto> UpdateAsync(int id, CreateUpdateOrderDto input)
    {
        var order = await _orderRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Order not found.");
        var branch = await ValidateInputAsync(input); _mapper.Map(input, order); ApplyTotals(order, input, branch.TaxPercentage);
        foreach (var oldItem in order.Items.Where(x => !x.IsDeleted).ToList()) { oldItem.IsDeleted = true; oldItem.UpdatedDate = DateTime.UtcNow; }
        foreach (var item in input.Items) order.Items.Add(MapItem(item));
        order.UpdatedDate = DateTime.UtcNow;
        return _mapper.Map<OrderResponseDto>(await _orderRepository.UpdateAsync(order));
    }
    public async Task DeleteAsync(int id) => await _orderRepository.DeleteAsync(await _orderRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Order not found."));

    private async Task<Branch> ValidateInputAsync(CreateUpdateOrderDto input)
    {
        var branch = await _branchRepository.GetByIdAsync(input.BranchId) ?? throw new KeyNotFoundException("Branch not found.");
        if (input.CustomerId.HasValue && !await _context.Customers.AnyAsync(x => x.Id == input.CustomerId.Value)) throw new KeyNotFoundException("Customer not found.");
        if (input.AddressId.HasValue && !await _context.CustomerAddresses.AnyAsync(x => x.Id == input.AddressId.Value && (!input.CustomerId.HasValue || x.CustomerId == input.CustomerId.Value))) throw new KeyNotFoundException("Customer address not found.");
        if (input.OrderType == Domain.Entities.Enums.OrderType.DineIn)
        {
            if (!input.TableId.HasValue) throw new InvalidOperationException("TableId is required for dine-in orders.");
            if (!branch.AcceptsDineIn) throw new InvalidOperationException("This branch does not accept dine-in orders.");
            var table = await _tableRepository.GetByIdAsync(input.TableId.Value);
            if (table == null || table.BranchId != input.BranchId || !table.IsActive) throw new InvalidOperationException("The selected table is invalid for this branch.");
        }
        if (input.OrderType == Domain.Entities.Enums.OrderType.Delivery)
        {
            if (!input.CustomerId.HasValue) throw new InvalidOperationException("CustomerId is required for delivery orders.");
            if (!input.AddressId.HasValue) throw new InvalidOperationException("AddressId is required for delivery orders.");
            if (!branch.AcceptsDelivery) throw new InvalidOperationException("This branch does not accept delivery orders.");
        }
        if (input.OrderType != Domain.Entities.Enums.OrderType.DineIn) input.TableId = null;
        if (input.Items.Count == 0) throw new InvalidOperationException("At least one order item is required.");
        return branch;
    }
    private static OrderItem MapItem(CreateUpdateOrderItemDto item) => new() { ProductId = item.ProductId, Quantity = item.Quantity, UnitPrice = item.UnitPrice, TotalPrice = item.Quantity * item.UnitPrice, SpecialInstructions = item.SpecialInstructions };
    private static void ApplyTotals(Order order, CreateUpdateOrderDto input, decimal? taxPercentage)
    {
        order.SubTotal = input.Items.Sum(x => x.Quantity * x.UnitPrice); order.DiscountAmount = Math.Min(Math.Max(input.DiscountAmount, 0), order.SubTotal);
        var taxableAmount = order.SubTotal - order.DiscountAmount; order.TaxAmount = Math.Round(taxableAmount * (taxPercentage ?? 0m) / 100m, 2);
        order.DeliveryCharge = input.OrderType == Domain.Entities.Enums.OrderType.Delivery ? Math.Max(0, input.DeliveryCharge) : 0; order.GrandTotal = taxableAmount + order.TaxAmount + order.DeliveryCharge;
    }
}
