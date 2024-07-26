namespace McHealthCare.Domain.Entities
{
    public partial class Speciality : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;
    }
}