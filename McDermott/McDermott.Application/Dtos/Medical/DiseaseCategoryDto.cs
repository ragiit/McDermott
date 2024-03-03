namespace McDermott.Application.Dtos.Medical
{
    public partial class DiseaseCategoryDto : IMapFrom<DiseaseCategory>
    {
        public int Id { get; set; }
        public string? ParentCategory { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }
}