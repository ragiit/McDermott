namespace McDermott.Domain.Entities
{
    public class MaintenanceRecord : BaseAuditableEntity
    {
        public long? MaintenanceId { get; set; }
        public long? ProductId { get; set; }
        public string? DocumentBase64 { get; set; }
        public string? DocumentName { get; set; }
        public string? SequenceProduct { get; set; }

        [SetToNull]
        public virtual Maintenance? Maintenance { get; set; }

        [SetToNull]
        public virtual Product? Product { get; set; }
    }
}