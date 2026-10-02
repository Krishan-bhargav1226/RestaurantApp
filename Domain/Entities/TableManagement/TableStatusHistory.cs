using Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class TableStatusHistory : BaseEntity
    {
        [Required]
        public int TableId { get; set; }

        [Required]
        public TableStatus Status { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? CreatedByUserId { get; set; }
    }
}