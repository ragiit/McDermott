using Mapster;
using McHealthCare.Domain.Entities.Medical;
using System.ComponentModel.DataAnnotations;

namespace McHealthCare.Application.Dtos.Medical
{
    public class ChronicCategoryDto : IMapFrom<ChronicCategory>
    {
        public Guid Id { get; set; }

        [StringLength(250)]
        public string? Name { get; set; }

        public string? Description { get; set; }
    }

    public class CreateUpdateChronicCategoryDto : IMapFrom<ChronicCategory>
    {
        public Guid Id { get; set; }

        [StringLength(250)]
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}