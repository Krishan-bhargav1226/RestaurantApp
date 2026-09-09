using Application.Dtos.CustomerAddresses;
using AutoMapper;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Repositories.CustomerAddresses;
using Microsoft.EntityFrameworkCore;

namespace Application.Applications.CustomerAddresses
{
    public class CustomerAddressApplication : ICustomerAddressApplication
    {
        private readonly ICustomerAddressRepository _customerAddressRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CustomerAddressApplication(
            ICustomerAddressRepository customerAddressRepository,
            DataContext context,
            IMapper mapper)
        {
            _customerAddressRepository = customerAddressRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CustomerAddressDto>> GetByCustomerIdAsync(int customerId)
        {
            var addresses = await _customerAddressRepository.GetByCustomerIdAsync(customerId);

            return _mapper.Map<List<CustomerAddressDto>>(addresses);
        }

        public async Task<CustomerAddressDto> GetByIdAsync(int id)
        {
            var address = await _customerAddressRepository.GetByIdAsync(id);

            if (address == null)
            {
                throw new KeyNotFoundException("Customer address not found.");
            }

            return _mapper.Map<CustomerAddressDto>(address);
        }

        public async Task<CustomerAddressDto> CreateAsync(CreateUpdateCustomerAddressDto input)
        {
            var customerExists = await _context.Customers
                .AnyAsync(x => x.Id == input.CustomerId);

            if (!customerExists)
            {
                throw new KeyNotFoundException("Customer not found.");
            }

            if (input.IsDefault)
            {
                await ClearDefaultAsync(input.CustomerId);
            }

            var address = _mapper.Map<CustomerAddress>(input);
            var createdAddress = await _customerAddressRepository.CreateAsync(address);

            return _mapper.Map<CustomerAddressDto>(createdAddress);
        }

        public async Task<CustomerAddressDto> UpdateAsync(
            int id,
            CreateUpdateCustomerAddressDto input)
        {
            var address = await _customerAddressRepository.GetByIdAsync(id);

            if (address == null)
            {
                throw new KeyNotFoundException("Customer address not found.");
            }

            var customerExists = await _context.Customers
                .AnyAsync(x => x.Id == input.CustomerId);

            if (!customerExists)
            {
                throw new KeyNotFoundException("Customer not found.");
            }

            if (input.IsDefault)
            {
                await ClearDefaultAsync(input.CustomerId, id);
            }

            _mapper.Map(input, address);
            address.UpdatedDate = DateTime.UtcNow;

            var updatedAddress = await _customerAddressRepository.UpdateAsync(address);

            return _mapper.Map<CustomerAddressDto>(updatedAddress);
        }

        public async Task DeleteAsync(int id)
        {
            var address = await _customerAddressRepository.GetByIdAsync(id);

            if (address == null)
            {
                throw new KeyNotFoundException("Customer address not found.");
            }

            await _customerAddressRepository.DeleteAsync(address);
        }

        private async Task ClearDefaultAsync(int customerId, int? exceptId = null)
        {
            var addresses = await _context.CustomerAddresses
                .Where(x =>
                    x.CustomerId == customerId &&
                    (!exceptId.HasValue || x.Id != exceptId.Value) &&
                    x.IsDefault)
                .ToListAsync();

            foreach (var address in addresses)
            {
                address.IsDefault = false;
            }

            await _context.SaveChangesAsync();
        }
    }
}