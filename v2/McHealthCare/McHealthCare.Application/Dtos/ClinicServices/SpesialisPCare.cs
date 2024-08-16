using Newtonsoft.Json;

namespace McHealthCare.Application.Dtos.Transaction
{
    public class SpesialisPCare
    {
        [JsonProperty("kdSpesialis")]
        public string KdSpesialis { get; set; }

        [JsonProperty("nmSpesialis")]
        public string NmSpesialis { get; set; }
    }
}