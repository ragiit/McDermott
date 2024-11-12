using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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