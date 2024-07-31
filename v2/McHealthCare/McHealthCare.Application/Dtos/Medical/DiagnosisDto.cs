using Mapster;
using McHealthCare.Domain.Common;
using McHealthCare.Domain.Entities.Medical;
using System.ComponentModel.DataAnnotations;

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

        [SetToNull]
        public virtual DiseaseCategoryDto? DiseaseCategory { get; set; }
        [SetToNull]
        public virtual ChronicCategoryDto? ChronicCategory { get; set; }
    }

    public class createUpdateDiagnosisDto : IMapFrom<Diagnosis>
    {
        public Guid Id { get; set; }
        [StringLength(200)]
        public string? Name { get; set; }
        [StringLength(50)]
        public string? Code { get; set; }
        public Guid? DiseaseCategoryId { get; set; }
        public Guid? ChronicCategoryId { get; set; }

        [SetToNull]
        public virtual DiseaseCategoryDto? DiseaseCategory { get; set; }
        [SetToNull]
        public virtual ChronicCategoryDto? ChronicCategory { get; set; }
    }
}