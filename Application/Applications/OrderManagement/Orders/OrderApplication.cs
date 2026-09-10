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
            var prices = await GetBranchProductPricesAsync(input.BranchId, input.Items);

            var order = _mapper.Map<Order>(input);
            order.OrderDate = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;
            order.TableNumber = input.TableNumber;

            ApplyServerCalculatedValues(order, input, branch, prices);

            foreach (var item in input.Items)
                order.Items.Add(MapItem(item, prices[item.ProductId]));

            return _mapper.Map<OrderResponseDto>(await _orderRepository.CreateAsync(order));
        }

        public async Task<List<OrderResponseDto>> GetAllAsync()
        {
            return _mapper.Map<List<OrderResponseDto>>(await _orderRepository.GetAllAsync());
        }

        public async Task<OrderResponseDto> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            return _mapper.Map<OrderResponseDto>(order);
        }

        public async Task<OrderResponseDto> UpdateAsync(int id, CreateUpdateOrderDto input)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            var branch = await ValidateInputAsync(input);
            var prices = await GetBranchProductPricesAsync(input.BranchId, input.Items);

            _mapper.Map(input, order);
            order.Status = input.Status;
            ApplyServerCalculatedValues(order, input, branch, prices);

            foreach (var oldItem in order.Items.Where(x => !x.IsDeleted).ToList())
            {
                oldItem.IsDeleted = true;
                oldItem.UpdatedDate = DateTime.UtcNow;
            }

            foreach (var item in input.Items)
                order.Items.Add(MapItem(item, prices[item.ProductId]));

            order.UpdatedDate = DateTime.UtcNow;

            return _mapper.Map<OrderResponseDto>(await _orderRepository.UpdateAsync(order));
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            await _orderRepository.DeleteAsync(order);
        }

        private async Task<Branch> ValidateInputAsync(CreateUpdateOrderDto input)
        {
            if (!Enum.IsDefined(input.OrderType))
                throw new InvalidOperationException("Invalid order type.");

            if (input.Items == null || input.Items.Count == 0)
                throw new InvalidOperationException("At least one order item is required.");

            var branch = await _branchRepository.GetByIdAsync(input.BranchId);
            if (branch == null)
                throw new KeyNotFoundException("Branch not found.");

            if (input.CustomerId.HasValue)
            {
                var customerExists = await _context.Customers.AnyAsync(x => x.Id == input.CustomerId.Value);
                if (!customerExists)
                    throw new KeyNotFoundException("Customer not found.");
            }

            if (input.AddressId.HasValue)
            {
                var addressExists = await _context.CustomerAddresses.AnyAsync(x =>
                    x.Id == input.AddressId.Value &&
                    (!input.CustomerId.HasValue || x.CustomerId == input.CustomerId.Value));

                if (!addressExists)
                    throw new KeyNotFoundException("Customer address not found.");
            }

            if (input.OrderType == OrderType.DineIn)
            {
                if (!input.TableId.HasValue)
                    throw new InvalidOperationException("TableId is required for dine-in orders.");

                if (!branch.AcceptsDineIn)
                    throw new InvalidOperationException("This branch does not accept dine-in orders.");

                var table = await _tableRepository.GetByIdAsync(input.TableId.Value);

                if (table == null || table.BranchId != input.BranchId || !table.IsActive)
                    throw new InvalidOperationException("The selected table is invalid for this branch.");

                input.AddressId = null;
                input.TableNumber = table.TableNumber;
            }
            else
            {
                input.TableId = null;
                input.TableNumber = null;
                input.AddressId = input.OrderType == OrderType.Delivery ? input.AddressId : null;
            }

            if (input.OrderType == OrderType.Delivery)
            {
                if (!input.CustomerId.HasValue)
                    throw new InvalidOperationException("CustomerId is required for delivery orders.");

                if (!input.AddressId.HasValue)
                    throw new InvalidOperationException("AddressId is required for delivery orders.");

                if (!branch.AcceptsDelivery)
                    throw new InvalidOperationException("This branch does not accept delivery orders.");
            }

            return branch;
        }

        private async Task<Dictionary<int, decimal>> GetBranchProductPricesAsync(
            int branchId,
            IReadOnlyCollection<CreateUpdateOrderItemDto> items)
        {
            var productIds = items.Select(x => x.ProductId).Distinct().ToList();

            var branchProducts = await _context.BranchProducts
                .Where(x => x.BranchId == branchId && productIds.Contains(x.ProductId) && x.IsAvailable)
                .Select(x => new { x.ProductId, x.Price })
                .ToListAsync();

            var prices = branchProducts.ToDictionary(x => x.ProductId, x => x.Price);

            var missingProductIds = productIds.Where(id => !prices.ContainsKey(id)).ToList();
            if (missingProductIds.Count > 0)
                throw new InvalidOperationException("One or more selected products are not available at this branch.");

            return prices;
        }

        private static OrderItem MapItem(CreateUpdateOrderItemDto input, decimal unitPrice)
        {
            return new OrderItem
            {
                ProductId = input.ProductId,
                Quantity = input.Quantity,
                UnitPrice = unitPrice,
                TotalPrice = input.Quantity * unitPrice,
                SpecialInstructions = input.SpecialInstructions
            };
        }

        private static void ApplyServerCalculatedValues(
            Order order,
            CreateUpdateOrderDto input,
            Branch branch,
            IReadOnlyDictionary<int, decimal> prices)
        {
            var subtotal = input.Items.Sum(x => x.Quantity * prices[x.ProductId]);
            var discount = Math.Min(Math.Max(input.DiscountAmount, 0m), subtotal);
            var taxableAmount = subtotal - discount;
            var taxPercentage = Math.Max(0m, branch.TaxPercentage ?? 0m);

            if (taxPercentage > 100m)
                throw new InvalidOperationException("Branch tax percentage must be between 0 and 100.");

            order.SubTotal = subtotal;
            order.DiscountAmount = discount;
            order.TaxAmount = Math.Round(taxableAmount * taxPercentage / 100m, 2);
            order.DeliveryCharge = input.OrderType == OrderType.Delivery
                ? Math.Max(0m, branch.DeliveryCharge ?? 0m)
                : 0m;

            if (input.OrderType == OrderType.Delivery &&
                branch.MinDeliveryOrderAmount.HasValue &&
                taxableAmount < branch.MinDeliveryOrderAmount.Value)
            {
                throw new InvalidOperationException(
                    $"Minimum delivery order amount is {branch.MinDeliveryOrderAmount.Value:0.00}.");
            }

            order.GrandTotal = taxableAmount + order.TaxAmount + order.DeliveryCharge;
        }
    }
}
