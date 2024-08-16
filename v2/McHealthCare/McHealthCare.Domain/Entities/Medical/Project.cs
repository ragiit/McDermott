namespace McHealthCare.Domain.Entities.Medical
{
    public partial class Project : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}