namespace McDermott.Domain.Entities
{
    public class GeneralConsultanMedicalSupportLog : BaseAuditableEntity
    {
        public long? GeneralConsultanMedicalSupportId { get; set; }
        public long? UserById { get; set; }
        public EnumStatusGeneralConsultantServiceProcedureRoom Status { get; set; }
        public GeneralConsultanMedicalSupport? GeneralConsultanMedicalSupport { get; set; }
        public User? UserBy { get; set; }
    }
}