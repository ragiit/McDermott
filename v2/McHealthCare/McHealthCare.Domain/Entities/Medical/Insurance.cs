namespace McHealthCare.Domain.Entities.Medical
{
    public partial class Insurance : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Type { get; set; }
        public bool? IsBPJSKesehatan { get; set; }
        public bool? IsBPJSTK { get; set; }
        public int? AdminFee { get; set; }
        public int? Percentage { get; set; }
        public int? AdminFeeMax { get; set; }
    }
}