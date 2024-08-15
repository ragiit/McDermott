using McHealthCare.Domain.Entities.Medical;

namespace McHealthCare.Application.Dtos.Medical
{
    public class DiagnosisDto : IMapFrom<Diagnosis>
    {
        public Guid Id { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(50)]
        public string? Code { get; set; }

        public Guid? DiseaseCategoryId { get; set; }
        public Guid? ChronicCategoryId { get; set; }

        public virtual DiseaseCategoryDto? DiseaseCategory { get; set; }

        public virtual ChronicCategoryDto? ChronicCategory { get; set; }
    }

    public class CreateUpdateDiagnosisDto : IMapFrom<Diagnosis>
    {
        public Guid Id { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(50)]
        public string? Code { get; set; }

        public Guid? DiseaseCategoryId { get; set; }
        public Guid? ChronicCategoryId { get; set; }

        public virtual DiseaseCategoryDto? DiseaseCategory { get; set; }

        public virtual ChronicCategoryDto? ChronicCategory { get; set; }
    }
}