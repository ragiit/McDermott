namespace McHealthCare.Domain.Entities.Medical
{
    public partial class DiseaseCategory : BaseAuditableEntity
    {
        public Guid? ParentId { get; set; }
        public string? Name { get; set; }
        public string? ParentCategory { get; set; }

        public virtual DiseaseCategory? Parent { get; set; }
    }
}