namespace McHealthCare.Domain.Entities.Employees
{
    public class SickLeave : BaseAuditableEntity
    {
        public Guid? GeneralConsultanServiceId { get; set; }
        public string? TypeLeave { get; set; }
        public EnumStatusSickLeave Status { get; set; }

        public GeneralConsultanService? GeneralConsultanService { get; set; }
    }
}