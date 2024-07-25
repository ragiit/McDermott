namespace McHealthCare.Domain.Entities
{
    public class SickLeave : BaseAuditableEntity
    {
        public Guid? GeneralConsultansId { get; set; }
        public string? TypeLeave { get; set; }
        public EnumStatusSickLeave Status { get; set; }

        public GeneralConsultanService? GeneralConsultans { get; set; }
    }
}