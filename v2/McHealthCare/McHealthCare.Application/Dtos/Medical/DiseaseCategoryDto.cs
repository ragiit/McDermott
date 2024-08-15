using McHealthCare.Domain.Entities.Medical;

namespace McHealthCare.Application.Dtos.Medical
{
    public class DiseaseCategoryDto : IMapFrom<DiseaseCategory>
    {
        public Guid Id { get; set; }

        [StringLength(300)]
        public string? Name { get; set; }

        public string? ParentCategory { get; set; }
    }

    public class CreateUpdateDiseaseCategoryDto : IMapFrom<DiseaseCategory>
    {
        public Guid Id { get; set; }

        [StringLength(300)]
        public string? Name { get; set; }

        public string? ParentCategory { get; set; }
    }
}