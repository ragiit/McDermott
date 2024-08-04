namespace McHealthCare.Domain.Entities.Medical
{
    public partial class DiseaseCategory : BaseAuditableEntity
    {
        [StringLength(300)]
        public string? Name { get; set; }

        public string? ParentCategory { get; set; }
    }
}