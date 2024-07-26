namespace McHealthCare.Domain.Entities
{
    public partial class DiseaseCategory : BaseAuditableEntity
    {
        [StringLength(300)]
        public string Name { get; set; }

        public string? ParentCategory { get; set; }
    }
}