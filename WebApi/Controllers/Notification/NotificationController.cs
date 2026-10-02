using Domain.Entities;
using Infrastructure.Repositories.Parosa;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? userId, [FromQuery] int? customerId)
        {
            var notifications = await _notificationRepository.GetAllAsync();
            if (userId.HasValue)
                notifications = notifications.Where(n => n.UserId == userId.Value).ToList();
            if (customerId.HasValue)
                notifications = notifications.Where(n => n.CustomerId == customerId.Value).ToList();

            return Ok(notifications);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null) return NotFound(new { message = $"Notification with ID {id} not found" });
            return Ok(notification);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Notification notification)
        {
            notification.CreatedAt = DateTime.UtcNow;
            notification.SentAt = DateTime.UtcNow;
            await _notificationRepository.AddAsync(notification);
            return CreatedAtAction(nameof(GetById), new { id = notification.Id }, notification);
        }

        [HttpPut("{id:int}/mark-read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var existing = await _notificationRepository.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = $"Notification with ID {id} not found" });

            existing.IsRead = true;
            existing.UpdatedAt = DateTime.UtcNow;

            await _notificationRepository.UpdateAsync(existing);
            return Ok(existing);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _notificationRepository.GetByIdAsync(id);
            if (existing == null) return NotFound(new { message = $"Notification with ID {id} not found" });

            await _notificationRepository.DeleteAsync(existing);
            return NoContent();
        }
    }
}
