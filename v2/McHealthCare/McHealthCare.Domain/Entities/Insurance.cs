namespace McHealthCare.Domain.Entities
{
    public class Insurance : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string? Code { get; set; }

        public string? Type { get; set; }
        public bool IsBPJSKesehatan { get; set; }
        public bool IsBPJSTK { get; set; }
        public Guid? AdminFee { get; set; }
        public Guid? Presentase { get; set; }
        public Guid? AdminFeeMax { get; set; }
    }
}