using Newtonsoft.Json;

namespace McHealthCare.Application.Dtos.Transaction
{
    public class SpesialisRefrensiKhususPCare
    {
        [JsonProperty("kdKhusus")]
        public string KdKhusus { get; set; }

        [JsonProperty("nmKhusus")]
        public string NmKhusus { get; set; }
    }
}