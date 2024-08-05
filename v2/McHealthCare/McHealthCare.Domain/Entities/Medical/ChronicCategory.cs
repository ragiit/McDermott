namespace McHealthCare.Domain.Entities.Medical
{
    public partial class ChronicCategory : BaseAuditableEntity
    {
        [StringLength(250)]
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}