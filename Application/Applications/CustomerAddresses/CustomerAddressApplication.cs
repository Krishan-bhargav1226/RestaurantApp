using Application.Dtos.CustomerAddresses;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.CustomerAddresses;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Applications.CustomerAddresses;

public class CustomerAddressApplication : ICustomerAddressApplication
{
    private readonly ICustomerAddressRepository _repository;
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public CustomerAddressApplication(ICustomerAddressRepository repository, DataContext context, IMapper mapper) { _repository = repository; _context = context; _mapper = mapper; }
    public async Task<List<CustomerAddressDto>> GetByCustomerIdAsync(int customerId) => _mapper.Map<List<CustomerAddressDto>>(await _repository.GetByCustomerIdAsync(customerId));
    public async Task<CustomerAddressDto> GetByIdAsync(int id) => _mapper.Map<CustomerAddressDto>(await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Customer address not found."));
    public async Task<CustomerAddressDto> CreateAsync(CreateUpdateCustomerAddressDto input)
    {
        if (!await _context.Customers.AnyAsync(x => x.Id == input.CustomerId)) throw new KeyNotFoundException("Customer not found.");
        if (input.IsDefault) await ClearDefaultAsync(input.CustomerId);
        return _mapper.Map<CustomerAddressDto>(await _repository.CreateAsync(_mapper.Map<CustomerAddress>(input)));
    }
    public async Task<CustomerAddressDto> UpdateAsync(int id, CreateUpdateCustomerAddressDto input)
    {
        var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Customer address not found.");
        if (!await _context.Customers.AnyAsync(x => x.Id == input.CustomerId)) throw new KeyNotFoundException("Customer not found.");
        if (input.IsDefault) await ClearDefaultAsync(input.CustomerId, id);
        _mapper.Map(input, entity); entity.UpdatedDate = DateTime.UtcNow;
        return _mapper.Map<CustomerAddressDto>(await _repository.UpdateAsync(entity));
    }
    public async Task DeleteAsync(int id) => await _repository.DeleteAsync(await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Customer address not found."));
    private async Task ClearDefaultAsync(int customerId, int? exceptId = null)
    {
        var addresses = await _context.CustomerAddresses.Where(x => x.CustomerId == customerId && (!exceptId.HasValue || x.Id != exceptId.Value) && x.IsDefault).ToListAsync();
        foreach (var address in addresses) address.IsDefault = false;
        await _context.SaveChangesAsync();
    }
}
