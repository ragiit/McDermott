namespace McHealthCare.Domain.Entities.Medical
{
    public partial class Speciality : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}