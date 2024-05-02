namespace McDermott.Domain.Entities
{
    public class PatientAllergy : BaseAuditableEntity
    {
        public long UserId { get; set; }
        public string? Farmacology { get; set; }
        public string FarmacologiCode { get; set; } = string.Empty;
        public string? Weather { get; set; }
        public string WeatherCode { get; set; } = string.Empty;
        public string? Food { get; set; }
        public string FoodCode { get; set; } = string.Empty;

        [SetToNull]
        public virtual User? User { get; set; }
    }
}