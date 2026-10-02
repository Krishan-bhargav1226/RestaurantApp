using Domain.Entities;
using Infrastructure.Repositories.Parosa;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutletController : ControllerBase
    {
        private readonly IOutletRepository _outletRepository;

        public OutletController(IOutletRepository outletRepository)
        {
            _outletRepository = outletRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? venueId)
        {
            var outlets = await _outletRepository.GetAllAsync();
            if (venueId.HasValue)
                outlets = outlets.Where(o => o.VenueId == venueId.Value).ToList();

            return Ok(outlets);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var outlet = await _outletRepository.GetByIdAsync(id);
            if (outlet == null) return NotFound(new { message = $"Outlet with ID {id} not found" });
            return Ok(outlet);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Outlet outlet)
        {
            outlet.CreatedAt = DateTime.UtcNow;
            await _outletRepository.AddAsync(outlet);
            return CreatedAtAction(nameof(GetById), new { id = outlet.Id }, outlet);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Outlet outlet)
        {
            var existing = await _outletRepository.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = $"Outlet with ID {id} not found" });

            existing.VenueId = outlet.VenueId;
            existing.Code = outlet.Code;
            existing.Name = outlet.Name;
            existing.OutletType = outlet.OutletType;
            existing.IsActive = outlet.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            await _outletRepository.UpdateAsync(existing);
            return Ok(existing);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _outletRepository.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = $"Outlet with ID {id} not found" });

            await _outletRepository.DeleteAsync(existing);
            return NoContent();
        }
    }
}
