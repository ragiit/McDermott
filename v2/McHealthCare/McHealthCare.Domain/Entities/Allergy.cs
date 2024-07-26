using System.Text.Json.Serialization;

namespace McHealthCare.Domain.Entities
{
    public class Allergy : BaseAuditableEntity
    {
        public Guid Id { get; set; }
        [JsonPropertyName("kdAllergy")]
        public string KdAllergy { get; set; } = string.Empty;

        [JsonPropertyName("nmAllergy")]
        public string NmAllergy { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
    }
}
