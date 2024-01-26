using System.ComponentModel.DataAnnotations;

namespace McDermott.Application.Dtos
{
    public partial class DiagnosisDto : IMapFrom<Diagnosis>
    {
        public int Id { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; }

        [StringLength(50)]
        public string? Code { get; set; }

        public int? DiseaseCategoryId { get; set; }
        public int? CronisCategoryId { get; set; }

        public virtual DiseaseCategoryDto? DiseaseCategory { get; set; }
        public virtual CronisCategoryDto? CronisKategory { get; set; }
    }
}