using Application.Dtos.OrderItems;
using Application.Dtos.Orders;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Enums;
using Infrastructure;
using Infrastructure.Repositories.Branches;
using Infrastructure.Repositories.Orders;
using Infrastructure.Repositories.Tables;
using Microsoft.EntityFrameworkCore;

namespace Application.Applications.Orders
{
    public class OrderApplication : IOrderApplication
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly ITableRepository _tableRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public OrderApplication(
            IOrderRepository orderRepository,
            IBranchRepository branchRepository,
            ITableRepository tableRepository,
            DataContext context,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _branchRepository = branchRepository;
            _tableRepository = tableRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderResponseDto> CreateAsync(CreateUpdateOrderDto input)
        {
            var branch = await ValidateInputAsync(input);

            var order = _mapper.Map<Order>(input);
            order.OrderDate = DateTime.UtcNow;

            ApplyTotals(order, input, branch.TaxPercentage);

            foreach (var item in input.Items)
            {
                order.Items.Add(MapItem(item));
            }

            var createdOrder = await _orderRepository.CreateAsync(order);

            return _mapper.Map<OrderResponseDto>(createdOrder);
        }

        public async Task<List<OrderResponseDto>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return _mapper.Map<List<OrderResponseDto>>(orders);
        }

        public async Task<OrderResponseDto> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            return _mapper.Map<OrderResponseDto>(order);
        }

        public async Task<OrderResponseDto> UpdateAsync(
            int id,
            CreateUpdateOrderDto input)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            var branch = await ValidateInputAsync(input);

            _mapper.Map(input, order);
            ApplyTotals(order, input, branch.TaxPercentage);

            foreach (var oldItem in order.Items.Where(x => !x.IsDeleted).ToList())
            {
                oldItem.IsDeleted = true;
                oldItem.UpdatedDate = DateTime.UtcNow;
            }

            foreach (var item in input.Items)
            {
                order.Items.Add(MapItem(item));
            }

            order.UpdatedDate = DateTime.UtcNow;

            var updatedOrder = await _orderRepository.UpdateAsync(order);

            return _mapper.Map<OrderResponseDto>(updatedOrder);
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            await _orderRepository.DeleteAsync(order);
        }

        private async Task<Branch> ValidateInputAsync(CreateUpdateOrderDto input)
        {
            var branch = await _branchRepository.GetByIdAsync(input.BranchId);

            if (branch == null)
            {
                throw new KeyNotFoundException("Branch not found.");
            }

            if (input.CustomerId.HasValue)
            {
                var customerExists = await _context.Customers
                    .AnyAsync(x => x.Id == input.CustomerId.Value);

                if (!customerExists)
                {
                    throw new KeyNotFoundException("Customer not found.");
                }
            }

            if (input.AddressId.HasValue)
            {
                var addressExists = await _context.CustomerAddresses
                    .AnyAsync(x =>
                        x.Id == input.AddressId.Value &&
                        (!input.CustomerId.HasValue || x.CustomerId == input.CustomerId.Value));

                if (!addressExists)
                {
                    throw new KeyNotFoundException("Customer address not found.");
                }
            }

            if (input.OrderType == OrderType.DineIn)
            {
                if (!input.TableId.HasValue)
                {
                    throw new InvalidOperationException("TableId is required for dine-in orders.");
                }

                if (!branch.AcceptsDineIn)
                {
                    throw new InvalidOperationException("This branch does not accept dine-in orders.");
                }

                var table = await _tableRepository.GetByIdAsync(input.TableId.Value);

                if (table == null ||
                    table.BranchId != input.BranchId ||
                    !table.IsActive)
                {
                    throw new InvalidOperationException("The selected table is invalid for this branch.");
                }

                input.AddressId = null;
            }
            else
            {
                input.TableId = null;
                input.TableNumber = null;
            }

            if (input.OrderType == OrderType.Delivery)
            {
                if (!input.CustomerId.HasValue)
                {
                    throw new InvalidOperationException("CustomerId is required for delivery orders.");
                }

                if (!input.AddressId.HasValue)
                {
                    throw new InvalidOperationException("AddressId is required for delivery orders.");
                }

                if (!branch.AcceptsDelivery)
                {
                    throw new InvalidOperationException("This branch does not accept delivery orders.");
                }
            }

            if (input.Items == null || input.Items.Count == 0)
            {
                throw new InvalidOperationException("At least one order item is required.");
            }

            return branch;
        }

        private static OrderItem MapItem(CreateUpdateOrderItemDto input)
        {
            return new OrderItem
            {
                ProductId = input.ProductId,
                Quantity = input.Quantity,
                UnitPrice = input.UnitPrice,
                TotalPrice = input.Quantity * input.UnitPrice,
                SpecialInstructions = input.SpecialInstructions
            };
        }

        private static void ApplyTotals(
            Order order,
            CreateUpdateOrderDto input,
            decimal? taxPercentage)
        {
            order.SubTotal = input.Items.Sum(x => x.Quantity * x.UnitPrice);

            order.DiscountAmount = Math.Min(
                Math.Max(input.DiscountAmount, 0),
                order.SubTotal);

            var taxableAmount = order.SubTotal - order.DiscountAmount;

            order.TaxAmount = Math.Round(
                taxableAmount * (taxPercentage ?? 0m) / 100m,
                2);

            if (input.OrderType == OrderType.Delivery)
            {
                order.DeliveryCharge = Math.Max(0, input.DeliveryCharge);
            }
            else
            {
                order.DeliveryCharge = 0;
            }

            order.GrandTotal =
                taxableAmount +
                order.TaxAmount +
                order.DeliveryCharge;
        }
    }
}