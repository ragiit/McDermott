namespace McHealthCare.Domain.Entities.Medical{
    public partial class Diagnosis : BaseAuditableEntity{
        [StringLength(200)]
        public string? Name { get; set; }
        [StringLength(50)]
        public string? Code { get; set; }
        public Guid? DiseaseCategoryId { get; set; }
        public Guid? ChronicCategoryId { get; set; }

        [SetToNull]
        public virtual DiseaseCategory? DiseaseCategory { get; set; }
        [SetToNull]
        public virtual ChronicCategory? ChronicCategory { get; set; }
    }
}