using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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