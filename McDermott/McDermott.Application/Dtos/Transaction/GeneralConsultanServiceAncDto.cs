namespace McDermott.Application.Dtos.Transaction
{
    public class GeneralConsultanServiceAncDto : IMapFrom<GeneralConsultanServiceAnc>
    {
        public long Id { get; set; }
        public long GeneralConsultanServiceId { get; set; }
        public long PatientId { get; set; }
        public string Reference { get; set; } = string.Empty;

        [Required]
        public string? PregnancyStatusG { get; set; }

        [Required]
        public string? PregnancyStatusP { get; set; }

        [Required]
        public string? PregnancyStatusA { get; set; }

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
        public string? PregnancyStatusG { get; set; }
        public string? PregnancyStatusP { get; set; }
        public string? PregnancyStatusA { get; set; }
        public string? HPHT { get; set; }
        public string? HPL { get; set; }
    }
}