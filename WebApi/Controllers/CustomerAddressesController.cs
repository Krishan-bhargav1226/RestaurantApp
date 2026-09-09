using Application.Applications.CustomerAddresses;
using Application.Dtos.CustomerAddresses;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerAddressesController : ControllerBase
{
    private readonly ICustomerAddressApplication _application;
    public CustomerAddressesController(ICustomerAddressApplication application) => _application = application;
    [HttpGet("customer/{customerId:int}")] public async Task<IActionResult> GetByCustomer(int customerId) => Ok(await _application.GetByCustomerIdAsync(customerId));
    [HttpGet("{id:int}")] public async Task<IActionResult> GetById(int id) => Ok(await _application.GetByIdAsync(id));
    [HttpPost] public async Task<IActionResult> Create(CreateUpdateCustomerAddressDto input) => Ok(await _application.CreateAsync(input));
    [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id, CreateUpdateCustomerAddressDto input) => Ok(await _application.UpdateAsync(id, input));
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) { await _application.DeleteAsync(id); return NoContent(); }
}
