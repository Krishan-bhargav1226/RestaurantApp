using System.Security.Claims;
using Application.Applications.Orders;
using Application.Dtos.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,BranchAdmin,Manager,Staff")]
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
            input.CreatedByUserId = GetAuthenticatedUserId();
            return Ok(await _orderApplication.CreateAsync(input));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _orderApplication.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _orderApplication.GetByIdAsync(id));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateOrderDto input)
        {
            input.CreatedByUserId = GetAuthenticatedUserId();
            return Ok(await _orderApplication.UpdateAsync(id, input));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderApplication.DeleteAsync(id);
            return NoContent();
        }

        private int GetAuthenticatedUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out var id) || id <= 0)
                throw new UnauthorizedAccessException("Authenticated user identity is invalid.");

            return id;
        }
    }
}