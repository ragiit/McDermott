namespace McHealthCare.Domain.Entities
{
    public partial class Concoction : BaseAuditableEntity
    {
        public Guid? PharmacyId { get; set; }
        public Guid? MedicamentGroupId { get; set; }
        public Guid? PractitionerId { get; set; }
        public Guid? DrugFormId { get; set; }
        public Guid? UomId { get; set; }
        public Guid? Qty { get; set; }
        public Guid? QtyByDay { get; set; }
        public Guid? Day { get; set; }
        public Guid? TotalQty { get; set; }
        public string? MedicamenName { get; set; }

        public MedicamentGroup? MedicamentGroup { get; set; }
        public Pharmacy? Pharmacy { get; set; }

        // public User? Practitioner { get; set; }
        public DrugForm? DruForm { get; set; }

        public Uom? Uom { get; set; }
    }
}