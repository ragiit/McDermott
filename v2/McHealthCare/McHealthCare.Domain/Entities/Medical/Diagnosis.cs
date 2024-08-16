namespace McHealthCare.Domain.Entities.Medical
{
    public partial class Diagnosis : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public Guid? DiseaseCategoryId { get; set; }
        public Guid? ChronicCategoryId { get; set; }

        public virtual DiseaseCategory? DiseaseCategory { get; set; }

        public virtual ChronicCategory? ChronicCategory { get; set; }
    }
}