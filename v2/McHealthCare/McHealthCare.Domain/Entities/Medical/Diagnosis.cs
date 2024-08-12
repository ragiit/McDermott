namespace McHealthCare.Domain.Entities.Medical
{
    public partial class Diagnosis : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public Guid? DiseaseCategoryId { get; set; }
        public Guid? ChronicCategoryId { get; set; }

        [SetToNull]
        public virtual DiseaseCategory? DiseaseCategory { get; set; }
        [SetToNull]
        public virtual ChronicCategory? ChronicCategory { get; set; }
    }
}