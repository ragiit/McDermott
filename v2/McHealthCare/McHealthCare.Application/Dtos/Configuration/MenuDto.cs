using Mapster;
using McHealthCare.Domain.Entities;
using McHealthCare.Domain.Entities.Configuration;
using System.ComponentModel.DataAnnotations;

namespace McHealthCare.Application.Dtos.Configuration
{
    public class MenuDto : IMapFrom<Menu>
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public long Sequence { get; set; } = 0;
        public string? Url { get; set; }

        public MenuDto? Parent { get; set; }

    }

    public class CreateUpdateMenuDto : IMapFrom<Menu>
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public long Sequence { get; set; } = 0;
        public string? Url { get; set; }

    }
}