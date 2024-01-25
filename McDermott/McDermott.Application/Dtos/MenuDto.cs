using System.ComponentModel.DataAnnotations;

namespace McDermott.Application.Dtos
{
    public class MenuDto : IMapFrom<Menu>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Icon { get; set; }
        public string? ParentMenu { get; set; }
        public int? Sequence { get; set; }
        public string? Html { get; set; }
        public string? Url { get; set; }
    }
}