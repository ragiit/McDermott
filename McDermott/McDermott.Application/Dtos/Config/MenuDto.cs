namespace McDermott.Application.Dtos.Config
{
    public class MenuDto : IMapFrom<Menu>
    {
        public long Id { get; set; }

        public long? ParentId { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Icon { get; set; }
        public long? Sequence { get; set; } = 1;
        public string? Url { get; set; }
        public bool IsDefaultData { get; set; }

        public MenuDto? Parent { get; set; }
    }

    public class CreateUpdateMenuDto
    {
        public long Id { get; set; }

        public long? ParentId { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? Icon { get; set; }
        public long? Sequence { get; set; }
        public string? Url { get; set; }
        public bool IsDefaultData { get; set; }
    }
}