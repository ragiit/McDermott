namespace McDermott.Domain.Entities
{
    public class SickLeave : BaseAuditableEntity
    {
        public long? GeneralConsultansId { get; set; }
        public string? TypeLeave { get; set; }
        public EnumStatusSickLeave Status { get; set; }

        public GeneralConsultanService? GeneralConsultans { get; set; }
    }
}