namespace McDermott.Domain.Entities
{
    public partial class Service : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;

        public string Quota { get; set; } = string.Empty;
        public bool IsPatient { get; set; }
        public bool IsKiosk { get; set; }
        public bool IsMcu { get; set; } = false;
        public bool IsVaccination { get; set; } = false;
        public bool IsTelemedicine { get; set; } = false;
        public bool IsMaternity { get; set; } = false;
        public long? ServicedId { get; set; }

        [SetToNull]
        public virtual Service? Serviced { get; set; }
    }
}