namespace McDermott.Domain.Entities
{
    public partial class DiseaseCategory : BaseAuditableEntity
    {
        [Required]
        [StringLength(300)]
        public string Name { get; set; } = string.Empty;

        public long? ParentDiseaseCategoryId { get; set; }

        public virtual DiseaseCategory? ParentDiseaseCategory { get; set; }
    }
}