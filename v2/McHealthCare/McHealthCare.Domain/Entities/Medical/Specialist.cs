namespace McHealthCare.Domain.Entities.Medical
{
    public partial class Specialist : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}