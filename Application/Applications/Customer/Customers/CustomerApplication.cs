using Application.Dtos.Customers;
using Application.DTOs.Customers;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Customers;

namespace Application.Applications.Customers;

public class CustomerApplication : ICustomerApplication
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerApplication(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CustomerResponseDto> CreateAsync(CreateUpdateCustomerDto input)
    {
        var customer = _mapper.Map<Customer>(input);
        customer.Email = customer.Email.Trim().ToLowerInvariant();
        customer.Phone = customer.Phone.Trim();

        var createdCustomer = await _customerRepository.CreateAsync(customer);
        return _mapper.Map<CustomerResponseDto>(createdCustomer);
    }

    public async Task<List<CustomerResponseDto>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return _mapper.Map<List<CustomerResponseDto>>(customers);
    }

    public async Task<CustomerResponseDto> GetByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer == null)
            throw new KeyNotFoundException("Customer not found.");

        return _mapper.Map<CustomerResponseDto>(customer);
    }

    public async Task<CustomerResponseDto> UpdateAsync(int id, CreateUpdateCustomerDto input)
    {
        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer == null)
            throw new KeyNotFoundException("Customer not found.");

        _mapper.Map(input, customer);
        customer.Email = customer.Email.Trim().ToLowerInvariant();
        customer.Phone = customer.Phone.Trim();
        customer.UpdatedDate = DateTime.UtcNow;

        var updatedCustomer = await _customerRepository.UpdateAsync(customer);
        return _mapper.Map<CustomerResponseDto>(updatedCustomer);
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer == null)
            throw new KeyNotFoundException("Customer not found.");

        await _customerRepository.DeleteAsync(customer);
    }
}
