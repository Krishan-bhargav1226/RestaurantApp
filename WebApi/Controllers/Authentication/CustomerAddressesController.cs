using System.Security.Claims;
using Application.Applications.CustomerAddresses;
using Application.Dtos.CustomerAddresses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomerAddressesController : ControllerBase
{
    private readonly ICustomerAddressApplication _customerAddressApplication;

    public CustomerAddressesController(ICustomerAddressApplication customerAddressApplication)
    {
        _customerAddressApplication = customerAddressApplication;
    }

    [HttpGet("customer/{customerId:int}")]
    public async Task<IActionResult> GetByCustomer(int customerId)
    {
        if (!CanAccessCustomer(customerId))
            return Forbid();

        var result = await _customerAddressApplication.GetByCustomerIdAsync(customerId);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _customerAddressApplication.GetByIdAsync(id);

        if (!CanAccessCustomer(result.CustomerId))
            return Forbid();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUpdateCustomerAddressDto input)
    {
        if (!CanAccessCustomer(input.CustomerId))
            return Forbid();

        var result = await _customerAddressApplication.CreateAsync(input);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateCustomerAddressDto input)
    {
        var existing = await _customerAddressApplication.GetByIdAsync(id);

        if (!CanAccessCustomer(existing.CustomerId) || existing.CustomerId != input.CustomerId)
            return Forbid();

        var result = await _customerAddressApplication.UpdateAsync(id, input);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _customerAddressApplication.GetByIdAsync(id);

        if (!CanAccessCustomer(existing.CustomerId))
            return Forbid();

        await _customerAddressApplication.DeleteAsync(id);
        return NoContent();
    }

    private bool CanAccessCustomer(int customerId)
    {
        var isCustomer = User.FindFirstValue("IsCustomer");
        if (!string.Equals(isCustomer, "True", StringComparison.OrdinalIgnoreCase))
            return true;

        return int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var currentCustomerId)
            && currentCustomerId == customerId;
    }
}