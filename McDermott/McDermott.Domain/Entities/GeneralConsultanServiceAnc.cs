namespace McDermott.Domain.Entities
{
    public partial class GeneralConsultanServiceAnc : BaseAuditableEntity
    {
        public long GeneralConsultanServiceId { get; set; }
        public long PatientId { get; set; }
        public string Reference { get; set; } = string.Empty;
        public int? PregnancyStatusG { get; set; }
        public int? PregnancyStatusP { get; set; }
        public int? PregnancyStatusA { get; set; }
        public int? PregnancyStatusH { get; set; }
        public string? HistorySC { get; set; }
        public int? UK { get; set; }
        public string? Notes { get; set; }
        public EnumStatusGeneralConsultanServiceAnc Status { get; set; } = EnumStatusGeneralConsultanServiceAnc.Open;
        public DateTime? HPHT { get; set; }
        public DateTime? HPL { get; set; }
        public string? LILA { get; set; }

        public GeneralConsultanService? GeneralConsultanService { get; set; }
        public User? Patient { get; set; }
    }
}