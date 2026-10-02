using Domain.Entities;
using Infrastructure.Repositories.Parosa;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionRepository _sectionRepository;

        public SectionController(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? outletId)
        {
            var sections = await _sectionRepository.GetAllAsync();
            if (outletId.HasValue)
                sections = sections.Where(s => s.OutletId == outletId.Value).ToList();

            return Ok(sections);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var section = await _sectionRepository.GetByIdAsync(id);
            if (section == null) return NotFound(new { message = $"Section with ID {id} not found" });
            return Ok(section);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Section section)
        {
            section.CreatedAt = DateTime.UtcNow;
            await _sectionRepository.AddAsync(section);
            return CreatedAtAction(nameof(GetById), new { id = section.Id }, section);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Section section)
        {
            var existing = await _sectionRepository.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = $"Section with ID {id} not found" });

            existing.OutletId = section.OutletId;
            existing.Name = section.Name;
            existing.SectionType = section.SectionType;
            existing.DisplayOrder = section.DisplayOrder;
            existing.IsActive = section.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            await _sectionRepository.UpdateAsync(existing);
            return Ok(existing);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _sectionRepository.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = $"Section with ID {id} not found" });

            await _sectionRepository.DeleteAsync(existing);
            return NoContent();
        }
    }
}
