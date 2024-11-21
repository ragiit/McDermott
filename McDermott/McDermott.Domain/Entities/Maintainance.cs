namespace McDermott.Domain.Entities
{
    public class Maintenance : BaseAuditableEntity
    {
        public long? RequestById { get; set; }
        public long? LocationId { get; set; }
        public string? Title { get; set; }
        public string? Sequence { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public long? ResponsibleById { get; set; }
        public bool? isCorrective { get; set; }
        public bool? isPreventive { get; set; }
        public bool? isInternal { get; set; }
        public bool? isExternal { get; set; }
        public string? VendorBy { get; set; }
        public bool? Recurrent { get; set; }
        public int? RepeatNumber { get; set; }
        public string? RepeatWork { get; set; }
        public EnumStatusMaintenance? Status { get; set; }

        [SetToNull]
        public virtual User? RequestBy { get; set; }

        [SetToNull]
        public virtual User? ResponsibleBy { get; set; }

        [SetToNull]
        public virtual Locations? Location { get; set; }
    }
}