namespace McHealthCare.Domain.Entities.Medical
{
    public partial class Service : BaseAuditableEntity
    {    
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Quota { get; set; }
        public bool IsPatient { get; set; }
        public bool IsKiosk { get; set; }
        public bool IsMcu { get; set; } = false;
        public Guid? ServicedId { get; set; }

        [SetToNull]
        public virtual Service? Serviced { get; set; }
    }
}