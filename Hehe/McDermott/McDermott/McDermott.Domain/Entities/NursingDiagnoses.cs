namespace McDermott.Domain.Entities
{
    public class NursingDiagnoses : BaseAuditableEntity
    {
        public string Problem { get; set; } = string.Empty;
        public string? Code { get; set; }
    }
}