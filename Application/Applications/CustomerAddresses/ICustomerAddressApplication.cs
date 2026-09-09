using Application.Dtos.CustomerAddresses;

namespace Application.Applications.CustomerAddresses;

public interface ICustomerAddressApplication
{
    Task<List<CustomerAddressDto>> GetByCustomerIdAsync(int customerId);
    Task<CustomerAddressDto> GetByIdAsync(int id);
    Task<CustomerAddressDto> CreateAsync(CreateUpdateCustomerAddressDto input);
    Task<CustomerAddressDto> UpdateAsync(int id, CreateUpdateCustomerAddressDto input);
    Task DeleteAsync(int id);
}
