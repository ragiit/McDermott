namespace McDermott.Application.Dtos.Medical
{
    public class InsuranceDto : IMapFrom<Insurance>
    {
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
        public bool? IsBPJS { get; set; } = false;
        public long? AdminFee { get; set; }
        public long? Presentase { get; set; }
        public long? AdminFeeMax { get; set; }
    }
}