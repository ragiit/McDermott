namespace McDermott.Domain.Entities
{
    public class Insurance : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Code { get; set; }

        public string? Type { get; set; }
        public bool IsBPJSKesehatan { get; set; }
        public bool IsBPJSTK { get; set; }
        public long? AdminFee { get; set; }
        public long? Presentase { get; set; }
        public long? AdminFeeMax { get; set; }
    }
}