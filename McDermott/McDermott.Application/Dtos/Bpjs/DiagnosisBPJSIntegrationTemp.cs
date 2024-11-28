using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Bpjs
{
    public class DiagnosisBPJSIntegrationTemp
    {
        [JsonPropertyName("kdDiag")]
        public string KdDiag { get; set; } = string.Empty;

        [JsonPropertyName("nmDiag")]
        public string NmDiag { get; set; } = string.Empty;

        [JsonPropertyName("nonSpesialis")]
        public bool NonSpesialis { get; set; }
    }
}