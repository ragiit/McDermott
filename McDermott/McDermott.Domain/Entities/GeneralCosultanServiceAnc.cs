namespace McDermott.Domain.Entities
{
    public class GeneralCosultanServiceAnc : BaseAuditableEntity
    {
        public long PatientId { get; set; }
        public long GeneralConsultanServiceId { get; set; } 
        public DateTime Date { get; set; }

        public User? User { get; set; }
        public GeneralConsultanService? GeneralConsultanService { get; set; }
    }
}
