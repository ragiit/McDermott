using Newtonsoft.Json;

namespace McDermott.Application.Dtos.Bpjs
{
    public class RujukLanjutRequest
    {
        [JsonProperty("tglEstRujuk")]
        public string TglEstRujuk { get; set; }

        [JsonProperty("kdppk")]
        public string Kdppk { get; set; }

        [JsonProperty("subSpesialis")]
        public object SubSpesialis { get; set; }

        [JsonProperty("khusus")]
        public KhususRequest Khusus { get; set; }
    }
}