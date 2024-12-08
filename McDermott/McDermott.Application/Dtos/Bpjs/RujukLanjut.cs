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
        public SubSpesialisRequestRujuk SubSpesialis { get; set; }

        [JsonProperty("khusus")]
        public KhususRequest Khusus { get; set; }
    }

    public class SubSpesialisRequestRujuk
    {
        [JsonProperty("kdSubSpesialis1")]
        public string KdSubSpesialis1 { get; set; }

        [JsonProperty("kdSarana")]
        public object KdSarana { get; set; }
    }
}