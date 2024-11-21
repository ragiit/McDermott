using Newtonsoft.Json;

namespace McDermott.Application.Dtos.Bpjs
{
    public class KhususRequest
    {
        [JsonProperty("kdKhusus")]
        public string KdKhusus { get; set; }

        [JsonProperty("kdSubSpesialis")]
        public object KdSubSpesialis { get; set; }

        [JsonProperty("catatan")]
        public string Catatan { get; set; }
    }
}