using Application.Applications.CustomerAddresses;
using Application.Dtos.CustomerAddresses;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAddressesController : ControllerBase
    {
        private readonly ICustomerAddressApplication _customerAddressApplication;

        public CustomerAddressesController(
            ICustomerAddressApplication customerAddressApplication)
        {
            _customerAddressApplication = customerAddressApplication;
        }

        [HttpGet("customer/{customerId:int}")]
        public async Task<IActionResult> GetByCustomer(int customerId)
        {
            var result = await _customerAddressApplication.GetByCustomerIdAsync(customerId);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _customerAddressApplication.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateCustomerAddressDto input)
        {
            var result = await _customerAddressApplication.CreateAsync(input);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] CreateUpdateCustomerAddressDto input)
        {
            var result = await _customerAddressApplication.UpdateAsync(id, input);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _customerAddressApplication.DeleteAsync(id);

            return NoContent();
        }
    }
}