using Newtonsoft.Json;

namespace McHealthCare.Application.Dtos.Transaction
{
    public class SubSpesialisPCare
    {
        [JsonProperty("kdSubSpesialis")]
        public string KdSubSpesialis { get; set; }

        [JsonProperty("nmSubSpesialis")]
        public string NmSubSpesialis { get; set; }

        [JsonProperty("kdPoliRujuk")]
        public string KdPoliRujuk { get; set; }
    }
}