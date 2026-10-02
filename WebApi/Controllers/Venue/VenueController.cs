using Domain.Entities;
using Infrastructure.Repositories.Parosa;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenueController : ControllerBase
    {
        private readonly IVenueRepository _venueRepository;

        public VenueController(IVenueRepository venueRepository)
        {
            _venueRepository = venueRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var venues = await _venueRepository.GetAllAsync();
            return Ok(venues);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var venue = await _venueRepository.GetByIdAsync(id);
            if (venue == null) return NotFound(new { message = $"Venue with ID {id} not found" });
            return Ok(venue);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Venue venue)
        {
            venue.CreatedAt = DateTime.UtcNow;
            await _venueRepository.AddAsync(venue);
            return CreatedAtAction(nameof(GetById), new { id = venue.Id }, venue);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Venue venue)
        {
            var existing = await _venueRepository.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = $"Venue with ID {id} not found" });

            existing.Code = venue.Code;
            existing.Name = venue.Name;
            existing.Slug = venue.Slug;
            existing.VenueType = venue.VenueType;
            existing.Description = venue.Description;
            existing.LogoUrl = venue.LogoUrl;
            existing.Phone = venue.Phone;
            existing.Email = venue.Email;
            existing.AddressLine1 = venue.AddressLine1;
            existing.AddressLine2 = venue.AddressLine2;
            existing.City = venue.City;
            existing.State = venue.State;
            existing.PostalCode = venue.PostalCode;
            existing.Country = venue.Country;
            existing.Currency = venue.Currency;
            existing.GSTIN = venue.GSTIN;
            existing.DefaultTaxPercentage = venue.DefaultTaxPercentage;
            existing.TimeZoneId = venue.TimeZoneId;
            existing.OperatingHoursJson = venue.OperatingHoursJson;
            existing.IsSuspended = venue.IsSuspended;
            existing.UpdatedAt = DateTime.UtcNow;

            await _venueRepository.UpdateAsync(existing);
            return Ok(existing);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _venueRepository.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = $"Venue with ID {id} not found" });

            await _venueRepository.DeleteAsync(existing);
            return NoContent();
        }
    }
}
