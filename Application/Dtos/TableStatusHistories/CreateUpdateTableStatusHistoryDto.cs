using System.ComponentModel.DataAnnotations;
using Domain.Entities.Enums;

namespace Application.Dtos.TableStatusHistories
{
    public class CreateUpdateTableStatusHistoryDto
    {
        [Range(1, int.MaxValue)]
        public int TableId { get; set; }

        [Required]
        public TableStatus Status { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? CreatedByUserId { get; set; }
    }
}