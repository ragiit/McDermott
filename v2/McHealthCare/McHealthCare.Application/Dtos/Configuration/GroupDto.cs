namespace McHealthCare.Application.Dtos.Configuration
{
    public class GroupDto : IMapFrom<Group>
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public bool IsDefaultData { get; set; } = false;
    }

    public class CreateUpdateGroupDto : IMapFrom<Group>
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }
}