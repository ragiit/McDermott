using System.Text.Json.Serialization;

namespace McHealthCare.Domain.Entities.ClinicService
{
    public partial class Awareness : BaseAuditableEntity
    {
        [JsonPropertyName("kdSadar")]
        public string KdSadar { get; set; } = string.Empty;

        [JsonPropertyName("nmSadar")]
        public string NmSadar { get; set; } = string.Empty;
         
    }
}