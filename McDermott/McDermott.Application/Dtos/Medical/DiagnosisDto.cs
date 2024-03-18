namespace McDermott.Application.Dtos.Medical
{
    public partial class DiagnosisDto : IMapFrom<Diagnosis>
    {
        public long Id { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; }

        [StringLength(50)]
        public string? Code { get; set; }

        public long? DiseaseCategoryId { get; set; }
        public long? CronisCategoryId { get; set; }

        public virtual DiseaseCategoryDto? DiseaseCategory { get; set; }
        public virtual CronisCategoryDto? CronisKategory { get; set; }
    }
}