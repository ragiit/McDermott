
namespace McDermott.Domain.Entities
{
    public class Product :BaseAuditableEntity
    {
        public string? Name { get; set; }

        public List<Medicament>? Medicaments { get; set; }
        public List<GeneralInformation>? GeneralInformation { get; set; }
    }
}
