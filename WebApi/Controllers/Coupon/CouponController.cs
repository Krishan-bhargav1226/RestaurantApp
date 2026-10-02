using Domain.Entities;
using Infrastructure.Repositories.Parosa;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;

        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var coupons = await _couponRepository.GetAllAsync();
            return Ok(coupons);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var coupon = await _couponRepository.GetByIdAsync(id);
            if (coupon == null) return NotFound(new { message = $"Coupon with ID {id} not found" });
            return Ok(coupon);
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var coupon = await _couponRepository.GetByCodeAsync(code);
            if (coupon == null) return NotFound(new { message = $"Coupon with code '{code}' not found or inactive" });
            return Ok(coupon);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Coupon coupon)
        {
            coupon.CreatedAt = DateTime.UtcNow;
            await _couponRepository.AddAsync(coupon);
            return CreatedAtAction(nameof(GetById), new { id = coupon.Id }, coupon);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Coupon coupon)
        {
            var existing = await _couponRepository.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = $"Coupon with ID {id} not found" });

            existing.Code = coupon.Code;
            existing.CouponType = coupon.CouponType;
            existing.Value = coupon.Value;
            existing.MinOrderAmount = coupon.MinOrderAmount;
            existing.MaxDiscountAmount = coupon.MaxDiscountAmount;
            existing.StartAt = coupon.StartAt;
            existing.EndAt = coupon.EndAt;
            existing.UsageLimit = coupon.UsageLimit;
            existing.UsedCount = coupon.UsedCount;
            existing.IsActive = coupon.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            await _couponRepository.UpdateAsync(existing);
            return Ok(existing);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _couponRepository.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = $"Coupon with ID {id} not found" });

            await _couponRepository.DeleteAsync(existing);
            return NoContent();
        }
    }
}
