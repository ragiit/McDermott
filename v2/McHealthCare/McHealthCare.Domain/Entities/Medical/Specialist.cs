namespace McHealthCare.Domain.Entities.Medical{
    public partial class Specialist : BaseAuditableEntity{
        [StringLength(200)]
        public string? Name { get; set; }
        [StringLength(5)]
        public string? Code { get; set; } 
    }
}