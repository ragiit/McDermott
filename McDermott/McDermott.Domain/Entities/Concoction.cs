namespace McDermott.Domain.Entities
{
    public partial class Concoction : BaseAuditableEntity
    {
        public long? PharmacyId { get; set; }
        public long? MedicamentGroupId { get; set; }
        public long? PractitionerId { get; set; }
        public long? DrugFormId { get; set; }
        public long? DrugRouteId { get; set; } 
        public long? DrugDosageId { get; set; }
        public long? ConcoctionQty { get; set; }
        public string? MedicamenName {  get; set; }
        public MedicamentGroup? MedicamentGroup { get; set; }
        public Pharmacy? Pharmacy { get; set; }
        public User? Practitioner { get; set; }
        public DrugForm? DrugForm { get; set; }
        public DrugRoute? DrugRoute { get; set; }
        public DrugDosage? DrugDosage{ get; set; }
    }
}