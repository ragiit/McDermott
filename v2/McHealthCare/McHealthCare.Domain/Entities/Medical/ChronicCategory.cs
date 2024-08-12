namespace McHealthCare.Domain.Entities.Medical
{
    public partial class ChronicCategory : BaseAuditableEntity
    {        
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}