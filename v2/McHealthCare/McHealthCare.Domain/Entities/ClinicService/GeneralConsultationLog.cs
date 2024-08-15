namespace McHealthCare.Domain.Entities.ClinicService
{
    public class GeneralConsultationLog : BaseAuditableEntity
    {
        public Guid? GeneralConsultanServiceId { get; set; }
        public Guid? ProcedureRoomId { get; set; }
        public string? UserById { get; set; }
        public string? Status { get; set; }

        public GeneralConsultanService? GeneralConsultanService { get; set; }
        public GeneralConsultanMedicalSupport? ProcedureRoom { get; set; }
        public ApplicationUser? UserBy { get; set; }
    }
}