namespace McHealthCare.Domain.Entities
{
    public class PatientAllergy : BaseAuditableEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string? Farmacology { get; set; }
        public string FarmacologiCode { get; set; } = string.Empty;
        public string? Weather { get; set; }
        public string WeatherCode { get; set; } = string.Empty;
        public string? Food { get; set; }
        public string FoodCode { get; set; } = string.Empty;

        public virtual Patient? Patient { get; set; }
    }
}