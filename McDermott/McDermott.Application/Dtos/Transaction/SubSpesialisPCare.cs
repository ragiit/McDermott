using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace McDermott.Application.Dtos.Transaction
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