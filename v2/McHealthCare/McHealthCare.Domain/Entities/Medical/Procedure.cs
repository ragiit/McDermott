namespace McHealthCare.Domain.Entities.Medical
{
    public partial class Procedure : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Classification { get; set; }
    }
}