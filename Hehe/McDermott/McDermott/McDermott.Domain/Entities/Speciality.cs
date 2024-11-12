namespace McDermott.Domain.Entities
{
    public partial class Speciality : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;
    }
}