using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Transaction
{
    public class SpesialisSaranaPCare
    {
        [JsonProperty("kdSarana")]
        public string KdSarana { get; set; }

        [JsonProperty("nmSarana")]
        public string NmSarana { get; set; }
    }
}