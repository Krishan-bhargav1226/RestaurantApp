using Domain.Entities;
using Infrastructure.Repositories.Parosa;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet("session/{sessionToken}")]
        public async Task<IActionResult> GetActiveBySession(string sessionToken)
        {
            var cart = await _cartRepository.GetActiveBySessionAsync(sessionToken);
            if (cart == null) return NotFound(new { message = $"No active cart found for session '{sessionToken}'" });
            return Ok(cart);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            if (cart == null) return NotFound(new { message = $"Cart with ID {id} not found" });
            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cart cart)
        {
            cart.CreatedAt = DateTime.UtcNow;
            await _cartRepository.AddAsync(cart);
            return CreatedAtAction(nameof(GetById), new { id = cart.Id }, cart);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cart cart)
        {
            var existing = await _cartRepository.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = $"Cart with ID {id} not found" });

            existing.SessionToken = cart.SessionToken;
            existing.SeatTableId = cart.SeatTableId;
            existing.CustomerId = cart.CustomerId;
            existing.Status = cart.Status;
            existing.CouponId = cart.CouponId;
            existing.Subtotal = cart.Subtotal;
            existing.DiscountAmount = cart.DiscountAmount;
            existing.TaxAmount = cart.TaxAmount;
            existing.Total = cart.Total;
            existing.UpdatedAt = DateTime.UtcNow;

            await _cartRepository.UpdateAsync(existing);
            return Ok(existing);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _cartRepository.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = $"Cart with ID {id} not found" });

            await _cartRepository.DeleteAsync(existing);
            return NoContent();
        }
    }
}
