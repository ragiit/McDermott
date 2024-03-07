namespace McDermott.Domain.Entities
{
    public class GeneralConsultanCPPT : BaseAuditableEntity
    {
        public long GeneralConsultanServiceId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;

        [SetToNull] // Tandai properti yang harus diatur ke null
        public virtual GeneralConsultanService? GeneralConsultanService { get; set; }
    }
}