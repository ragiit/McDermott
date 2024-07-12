using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace McDermott.Application.Dtos.Transaction
{
    public class SpesialisRefrensiKhususPCare
    {
        [JsonProperty("kdKhusus")]
        public string KdKhusus { get; set; }

        [JsonProperty("nmKhusus")]
        public string NmKhusus { get; set; }
    }
}