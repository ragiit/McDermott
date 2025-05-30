namespace McHealthCare.Domain.Entities.Medical
{
    public partial class Building : BaseAuditableEntity
    {
        public Guid? HealthCenterId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }

        public virtual HealthCenter? HealthCenter { get; set; }
        
    }
}