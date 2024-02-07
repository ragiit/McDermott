using System.ComponentModel.DataAnnotations;

namespace McDermott.Application.Dtos.Config
{
    public class GroupDto : IMapFrom<Group>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }
}