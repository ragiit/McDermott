namespace McHealthCare.Domain.Entities.ClinicService
{
    public class GeneralConsultanCPPT : BaseAuditableEntity
    {
        public Guid GeneralConsultanServiceId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;

        // Tandai properti yang harus diatur ke null
        public virtual GeneralConsultanService? GeneralConsultanService { get; set; }
    }
}