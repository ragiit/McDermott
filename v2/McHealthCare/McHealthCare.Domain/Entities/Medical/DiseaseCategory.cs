namespace McHealthCare.Domain.Entities.Medical
{
    public partial class DiseaseCategory : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public string? ParentCategory { get; set; }
    }
}