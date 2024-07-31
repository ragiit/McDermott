namespace McHealthCare.Domain.Entities.Medical{
    public partial class Procedure :BaseAuditableEntity{
        [StringLength(200)]
        public string? Name { get; set; }
        [StringLength(100)]
        public string? Code { get; set; }
        [StringLength(100)]
        public string? Classification { get; set; }
    }
}