namespace McDermott.Domain.Entities
{
    public class Insurance : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
        public bool IsBPJS { get; set; }
        public int? AdminFee { get; set; }
        public int? Presentase { get; set; }
        public int? AdminFeeMax { get; set; }
    }
}