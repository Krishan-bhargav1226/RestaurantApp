using System.Security.Claims;
using Application.Applications.CustomerAddresses;
using Application.Dtos.CustomerAddresses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,BranchAdmin,Manager,Staff,Customer")]
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
            if (!HasCustomerAccess(customerId))
                return Forbid();

            return Ok(await _customerAddressApplication.GetByCustomerIdAsync(customerId));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _customerAddressApplication.GetByIdAsync(id);

            if (!HasCustomerAccess(result.CustomerId))
                return Forbid();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateCustomerAddressDto input)
        {
            if (!HasCustomerAccess(input.CustomerId))
                return Forbid();

            return Ok(await _customerAddressApplication.CreateAsync(input));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateCustomerAddressDto input)
        {
            var existing = await _customerAddressApplication.GetByIdAsync(id);

            if (!HasCustomerAccess(existing.CustomerId))
                return Forbid();

            input.CustomerId = existing.CustomerId;
            return Ok(await _customerAddressApplication.UpdateAsync(id, input));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _customerAddressApplication.GetByIdAsync(id);

            if (!HasCustomerAccess(existing.CustomerId))
                return Forbid();

            await _customerAddressApplication.DeleteAsync(id);
            return NoContent();
        }

        private bool HasCustomerAccess(int customerId)
        {
            if (!User.IsInRole("Customer"))
                return true;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userId, out var authenticatedCustomerId) &&
                   authenticatedCustomerId == customerId;
        }
    }
}
