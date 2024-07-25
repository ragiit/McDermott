namespace McHealthCare.Domain.Entities
{
    public partial class Procedure : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(100)]
        public string? Code_Test { get; set; }

        [StringLength(100)]
        public string? Classification { get; set; }
    }
}