using Application.Dtos.Customers;
using Application.DTOs.Customers;

namespace Application.Applications.Customers;

public interface ICustomerApplication
{
    Task<CustomerResponseDto> CreateAsync(CreateUpdateCustomerDto input);
    Task<List<CustomerResponseDto>> GetAllAsync();
    Task<CustomerResponseDto> GetByIdAsync(int id);
    Task<CustomerResponseDto> UpdateAsync(int id, CreateUpdateCustomerDto input);
    Task DeleteAsync(int id);
}
