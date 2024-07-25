namespace McHealthCare.Domain.Entities
{
    public class DrugRoute : BaseAuditableEntity
    {
        public string Route { get; set; } = string.Empty;
        public string? Code { get; set; }

        public virtual List<DrugDosage>? DrugDosages { get; set; }
        public virtual List<Medicament>? Medicaments { get; set; }
    }
}   
