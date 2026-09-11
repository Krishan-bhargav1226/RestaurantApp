using Application.Applications.Orders;
using Application.Dtos.Orders;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class OrderController : ControllerBase
    {
        private readonly IOrderApplication _orderApplication;

        public OrderController(IOrderApplication orderApplication)
        {
            _orderApplication = orderApplication;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateOrderDto input)
        {
            var result = await _orderApplication.CreateAsync(input);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderApplication.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _orderApplication.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateOrderDto input)
        {
            var result = await _orderApplication.UpdateAsync(id, input);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderApplication.DeleteAsync(id);
            return NoContent();
        }
    }
}