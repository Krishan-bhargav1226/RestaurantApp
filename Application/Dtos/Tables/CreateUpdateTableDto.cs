using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Tables
{
    public class CreateUpdateTableDto
    {
        [Range(1, int.MaxValue)]
        public int BranchId { get; set; }

        [Required]
        [MaxLength(20)]
        public string TableNumber { get; set; } = string.Empty;

        [Range(1, 100)]
        public int Capacity { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }
    }
}