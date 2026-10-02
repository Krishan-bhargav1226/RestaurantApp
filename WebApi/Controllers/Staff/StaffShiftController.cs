using Domain.Entities;
using Infrastructure.Repositories.Parosa;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffShiftController : ControllerBase
    {
        private readonly IStaffShiftRepository _staffShiftRepository;

        public StaffShiftController(IStaffShiftRepository staffShiftRepository)
        {
            _staffShiftRepository = staffShiftRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? outletId, [FromQuery] int? userId)
        {
            var shifts = await _staffShiftRepository.GetAllAsync();
            if (outletId.HasValue)
                shifts = shifts.Where(s => s.OutletId == outletId.Value).ToList();
            if (userId.HasValue)
                shifts = shifts.Where(s => s.UserId == userId.Value).ToList();

            return Ok(shifts);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shift = await _staffShiftRepository.GetByIdAsync(id);
            if (shift == null) return NotFound(new { message = $"Staff shift with ID {id} not found" });
            return Ok(shift);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StaffShift shift)
        {
            shift.CreatedAt = DateTime.UtcNow;
            await _staffShiftRepository.AddAsync(shift);
            return CreatedAtAction(nameof(GetById), new { id = shift.Id }, shift);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] StaffShift shift)
        {
            var existing = await _staffShiftRepository.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = $"Staff shift with ID {id} not found" });

            existing.OutletId = shift.OutletId;
            existing.UserId = shift.UserId;
            existing.StartAt = shift.StartAt;
            existing.EndAt = shift.EndAt;
            existing.Status = shift.Status;
            existing.Notes = shift.Notes;
            existing.UpdatedAt = DateTime.UtcNow;

            await _staffShiftRepository.UpdateAsync(existing);
            return Ok(existing);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _staffShiftRepository.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = $"Staff shift with ID {id} not found" });

            await _staffShiftRepository.DeleteAsync(existing);
            return NoContent();
        }
    }
}
