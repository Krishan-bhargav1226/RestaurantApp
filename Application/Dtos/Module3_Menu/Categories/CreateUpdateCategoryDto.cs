using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Categories
{
    public class CreateUpdateCategoryDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? ImagePath { get; set; }

        [Range(0, int.MaxValue)]
        public int DisplayOrder { get; set; }
    }
}
