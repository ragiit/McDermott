namespace McHealthCare.Domain.Entities
{
    public class PatientAllergy : BaseAuditableEntity
    {
        public Guid UserId { get; set; }
        public string? Farmacology { get; set; }
        public string FarmacologiCode { get; set; } = string.Empty;
        public string? Weather { get; set; }
        public string WeatherCode { get; set; } = string.Empty;
        public string? Food { get; set; }
        public string FoodCode { get; set; } = string.Empty;

        
        // public virtual User? User { get; set; }
    }
}