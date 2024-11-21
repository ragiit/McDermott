namespace McDermott.Application.Dtos.Medical
{
    public partial class DiagnosisDto : IMapFrom<Diagnosis>
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? NameInd { get; set; }

        [StringLength(50)]
        public string? Code { get; set; }

        // Dibuang
        public long? DiseaseCategoryId { get; set; }

        public long? CronisCategoryId { get; set; }

        public virtual DiseaseCategoryDto? DiseaseCategory { get; set; }
        public virtual CronisCategoryDto? CronisCategory { get; set; }
    }

    public class CreateUpdateDiagnosisDto
    {
        public long Id { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? NameInd { get; set; }

        [StringLength(50)]
        public string? Code { get; set; }

        public long? DiseaseCategoryId { get; set; }
        public long? CronisCategoryId { get; set; }
    }
}