namespace McDermott.Application.Dtos.Transaction
{
    public class GeneralConsultanServiceAncDto : IMapFrom<GeneralConsultanServiceAnc>
    {
        public long Id { get; set; }
        public long GeneralConsultanServiceId { get; set; }
        public long PatientId { get; set; }
        public string Reference { get; set; } = string.Empty;
        public int? PregnancyStatusG { get; set; }
        public int? PregnancyStatusP { get; set; }
        public int? PregnancyStatusA { get; set; }
        public int? PregnancyStatusH { get; set; }
        public int? UK { get; set; }
        public string? HistorySC { get; set; }
        public string? Notes { get; set; }
        public EnumStatusGeneralConsultanServiceAnc Status { get; set; } = EnumStatusGeneralConsultanServiceAnc.Open;

        [Required]
        public DateTime? HPHT { get; set; }

        [Required]
        public DateTime? HPL { get; set; }

        public GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
        public User? Patient { get; set; }
    }

    public class CreateUpdateGeneralConsultanServiceAncDto
    {
        public long Id { get; set; }
        public long GeneralConsultanServiceId { get; set; }
        public long PatientId { get; set; }
        public string Reference { get; set; } = string.Empty;
        public int? PregnancyStatusG { get; set; }
        public int? PregnancyStatusP { get; set; }
        public int? PregnancyStatusA { get; set; }
        public int? PregnancyStatusH { get; set; }
        public string? HistorySC { get; set; }
        public string? Notes { get; set; }
        public EnumStatusGeneralConsultanServiceAnc Status { get; set; } = EnumStatusGeneralConsultanServiceAnc.Open;
        public int? UK { get; set; }
        public string? HPHT { get; set; }
        public string? HPL { get; set; }
    }
}