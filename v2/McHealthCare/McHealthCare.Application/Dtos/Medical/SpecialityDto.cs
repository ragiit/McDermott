using McHealthCare.Domain.Entities.Medical;

namespace McHealthCare.Application.Dtos.Medical
{
    public class SpecialityDto : IMapFrom<Speciality>
    {
        public Guid Id { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(5)]
        public string? Code { get; set; }
    }

    public class CreateUpdateSpecialityDto : IMapFrom<Speciality>
    {
        public Guid Id { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(5)]
        public string? Code { get; set; }
    }
}