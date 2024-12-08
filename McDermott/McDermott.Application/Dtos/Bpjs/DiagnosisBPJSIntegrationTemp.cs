using System.Text.Json.Serialization;

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