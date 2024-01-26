using System.ComponentModel.DataAnnotations;

namespace McDermott.Application.Dtos
{
    public partial class CronisCategoryDto:IMapFrom<ChronisCategory>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(300)]
        public string Description { get;set; }
    }
}
