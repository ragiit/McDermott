namespace McDermott.Domain.Entities
{
    public partial class Concoction : BaseAuditableEntity
    {
        public long? PharmacyId { get; set; }
        public long? MedicamentGroupId { get; set; }
        public long? PractitionerId { get; set; }
        public long? DrugFormId { get; set; }
        public long? UomId { get; set; }
        public long? Qty { get; set; }
        public long? QtyByDay { get; set; }
        public long? Day { get; set; }
        public long? TotalQty { get; set; }
        public string? MedicamenName {  get; set; }

        public MedicamentGroup? MedicamentGroup { get; set; }
        public Pharmacy? Pharmacy { get; set; }
        public User? Practitioner { get; set; }
        public DrugForm? DruForm { get; set; }
        public Uom? Uom { get; set; }
    }
}