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
        [SetToNull]
        public MedicamentGroup? MedicamentGroup { get; set; }
        [SetToNull]
        public Pharmacy? Pharmacy { get; set; }
        [SetToNull]
        public User? Practitioner { get; set; }
        [SetToNull]
        public DrugForm? DrugForm { get; set; }
        [SetToNull]
        public DrugRoute? DrugRoute { get; set; }
        [SetToNull]
        public DrugDosage? DrugDosage{ get; set; }
    }
}