namespace McDermott.Domain.Entities
{
    public class GeneralConsultationServiceLog : BaseAuditableEntity
    {
        public long? GeneralConsultanServiceId { get; set; }
        public long? UserById { get; set; }
        public EnumStatusGeneralConsultantService status { get; set; }

        public GeneralConsultanService? GeneralConsultanService { get; set; }
        public User? UserBy { get; set; }
    }
}