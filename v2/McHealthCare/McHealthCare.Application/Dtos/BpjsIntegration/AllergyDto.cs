using System.Text.Json.Serialization;

namespace McHealthCare.Application.Dtos.BpjsIntegration
{
    public class AllergyDto : IMapFrom<Allergy>
    {
        public Guid Id { get; set; }

        [JsonPropertyName("kdAllergy")]
        public string KdAllergy { get; set; } = string.Empty;

        [JsonPropertyName("nmAllergy")]
        public string NmAllergy { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty;

        [NotMapped]
        public string TypeString { get; set; } = string.Empty;
    }
}