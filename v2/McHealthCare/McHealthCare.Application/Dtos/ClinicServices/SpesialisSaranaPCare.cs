using Newtonsoft.Json;

namespace McHealthCare.Application.Dtos.Transaction
{
    public class SpesialisSaranaPCare
    {
        [JsonProperty("kdSarana")]
        public string KdSarana { get; set; }

        [JsonProperty("nmSarana")]
        public string NmSarana { get; set; }
    }
}