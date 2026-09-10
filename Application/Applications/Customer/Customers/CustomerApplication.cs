using System.Security.Cryptography;
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
        if (string.IsNullOrWhiteSpace(input.Password))
            throw new InvalidOperationException("Password is required when creating a customer.");

        var customer = _mapper.Map<Customer>(input);
        customer.Email = customer.Email.Trim().ToLowerInvariant();
        customer.Phone = string.IsNullOrWhiteSpace(customer.Phone) ? null : customer.Phone.Trim();
        customer.PasswordHash = HashPassword(input.Password);
        customer.IsEmailVerified = false;

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
        customer.Phone = string.IsNullOrWhiteSpace(customer.Phone) ? null : customer.Phone.Trim();

        if (!string.IsNullOrWhiteSpace(input.Password))
        {
            customer.PasswordHash = HashPassword(input.Password);
            customer.RefreshTokenHash = null;
            customer.RefreshTokenExpiry = null;
        }

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

    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100000, HashAlgorithmName.SHA256, 32);
        return $"100000.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }
}
