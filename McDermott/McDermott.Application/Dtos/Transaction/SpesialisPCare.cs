using Newtonsoft.Json;

namespace McDermott.Application.Dtos.Transaction
{
    public class SpesialisPCare
    {
        [JsonProperty("kdSpesialis")]
        public string KdSpesialis { get; set; }

        [JsonProperty("nmSpesialis")]
        public string NmSpesialis { get; set; }
    }
}