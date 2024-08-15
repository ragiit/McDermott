using System.Text.Json.Serialization;

namespace McHealthCare.Application.Dtos.BpjsIntegration
{
    public class AwarenessDto : IMapFrom<Awareness>
    {
        public long Id { get; set; }

        [JsonPropertyName("kdSadar")]
        public string KdSadar { get; set; } = string.Empty;

        [JsonPropertyName("nmSadar")]
        public string NmSadar { get; set; } = string.Empty;
    }
}