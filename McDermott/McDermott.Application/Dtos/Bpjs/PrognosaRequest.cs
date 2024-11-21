using Newtonsoft.Json;

namespace McDermott.Application.Dtos.Bpjs
{
    public class PrognosaRequest
    {
        [JsonProperty("kdPrognosa")]
        public string KdPrognosa { get; set; }

        [JsonProperty("nmPrognosa")]
        public string NmPrognosa { get; set; }
    }
}