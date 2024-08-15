

namespace McHealthCare.Domain.Entities.Pharmacies
{
    public partial class Concoction : BaseAuditableEntity
    {
        public Guid? PharmacyId { get; set; }
        public Guid? MedicamentGroupId { get; set; }
        public Guid? PractitionerId { get; set; }
        public Guid? DrugFormId { get; set; }
        public Guid? DrugRouteId { get; set; } 
        public Guid? DrugDosageId { get; set; }
        public Guid? ConcoctionQty { get; set; }
        public string? MedicamenName {  get; set; }
        public MedicamentGroup? MedicamentGroup { get; set; }
        public Pharmacy? Pharmacy { get; set; }
        public Doctor? Practitioner { get; set; }
        public DrugForm? DrugForm { get; set; }
        public DrugRoute? DrugRoute { get; set; }
        public DrugDosage? DrugDosage{ get; set; }
    }
}