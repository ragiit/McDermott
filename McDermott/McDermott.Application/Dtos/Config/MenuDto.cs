namespace McDermott.Application.Dtos.Config
{
    public class MenuDto : IMapFrom<Menu>
    {
        public long Id { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Icon { get; set; }
        public string? ParentMenu { get; set; }
        public long? Sequence { get; set; }
        public string? Html { get; set; }
        public string? Url { get; set; }
    }
}