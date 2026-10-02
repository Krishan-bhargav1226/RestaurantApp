using Domain.Entities;
using Infrastructure.Repositories.Parosa;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRCodeController : ControllerBase
    {
        private readonly IQRCodeRepository _qrCodeRepository;

        public QRCodeController(IQRCodeRepository qrCodeRepository)
        {
            _qrCodeRepository = qrCodeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? outletId, [FromQuery] int? seatTableId)
        {
            var qrCodes = await _qrCodeRepository.GetAllAsync();
            if (outletId.HasValue)
                qrCodes = qrCodes.Where(q => q.OutletId == outletId.Value).ToList();
            if (seatTableId.HasValue)
                qrCodes = qrCodes.Where(q => q.SeatTableId == seatTableId.Value).ToList();

            return Ok(qrCodes);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var qrCode = await _qrCodeRepository.GetByIdAsync(id);
            if (qrCode == null) return NotFound(new { message = $"QR Code with ID {id} not found" });
            return Ok(qrCode);
        }

        [HttpGet("token/{token}")]
        public async Task<IActionResult> GetByToken(string token)
        {
            var qrCode = await _qrCodeRepository.GetByTokenAsync(token);
            if (qrCode == null) return NotFound(new { message = $"QR Code with token '{token}' not found or inactive" });
            return Ok(qrCode);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QRCode qrCode)
        {
            if (string.IsNullOrWhiteSpace(qrCode.Token))
            {
                qrCode.Token = Guid.NewGuid().ToString("N");
            }
            qrCode.CreatedAt = DateTime.UtcNow;
            await _qrCodeRepository.AddAsync(qrCode);
            return CreatedAtAction(nameof(GetById), new { id = qrCode.Id }, qrCode);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] QRCode qrCode)
        {
            var existing = await _qrCodeRepository.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = $"QR Code with ID {id} not found" });

            existing.OutletId = qrCode.OutletId;
            existing.SeatTableId = qrCode.SeatTableId;
            existing.CodeType = qrCode.CodeType;
            existing.Status = qrCode.Status;
            existing.TargetUrl = qrCode.TargetUrl;
            existing.ImageBlobUrl = qrCode.ImageBlobUrl;
            existing.UpdatedAt = DateTime.UtcNow;

            await _qrCodeRepository.UpdateAsync(existing);
            return Ok(existing);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _qrCodeRepository.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = $"QR Code with ID {id} not found" });

            await _qrCodeRepository.DeleteAsync(existing);
            return NoContent();
        }
    }
}
