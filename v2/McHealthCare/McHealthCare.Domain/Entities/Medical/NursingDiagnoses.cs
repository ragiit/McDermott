namespace McHealthCare.Domain.Entities.Medical
{
    public partial class NursingDiagnoses : BaseAuditableEntity
    {
        public string? Problem { get; set; }
        public string? Code { get; set; }
    }
}