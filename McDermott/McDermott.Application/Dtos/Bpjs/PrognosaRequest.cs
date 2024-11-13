using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
