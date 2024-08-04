using Mapster;
using McHealthCare.Domain.Entities.Medical;
using System.ComponentModel.DataAnnotations;

namespace McHealthCare.Application.Dtos.Medical
{
    public class SpecialistDto : IMapFrom<Specialist>
    {
        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(5)]
        public string? Code { get; set; }
    }

    public class CreateUpdateSpecialistDto : IMapFrom<Specialist>
    {
        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(5)]
        public string? Code { get; set; }
    }
}