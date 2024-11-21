namespace McDermott.Domain.Entities
{
    public class MaintenanceProduct : BaseAuditableEntity
    {
        public long? MaintenanceId { get; set; }
        public long? ProductId { get; set; }
        public string? SerialNumber { get; set; }
        public string? Note { get; set; }
        public DateTime? Expired { get; set; }
        public EnumStatusMaintenance? Status { get; set; }

        [SetToNull]
        public Maintenance? Maintenance { get; set; }

        [SetToNull]
        public Product? Product { get; set; }
    }
}