namespace McDermott.Application.Dtos.Config
{
    public class GroupDto : IMapFrom<Group>
    {
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public bool IsDefaultData { get; set; }
    }
}