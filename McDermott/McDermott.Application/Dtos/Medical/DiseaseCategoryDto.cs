namespace McDermott.Application.Dtos.Medical
{
    public partial class DiseaseCategoryDto : IMapFrom<DiseaseCategory>
    {
        public long Id { get; set; }
        public long? ParentDiseaseCategoryId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public virtual DiseaseCategory? ParentDiseaseCategory { get; set; }
    }

    public partial class CreateUpdateDiseaseCategoryDto
    {
        public long Id { get; set; }
        public long? ParentDiseaseCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual DiseaseCategory? ParentDiseaseCategory { get; set; }
    }
}