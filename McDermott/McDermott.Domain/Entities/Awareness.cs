using System.Text.Json.Serialization;

namespace McDermott.Domain.Entities
{
    public partial class Awareness : BaseAuditableEntity
    {
        [JsonPropertyName("kdSadar")]
        public string KdSadar { get; set; } = string.Empty;

        [JsonPropertyName("nmSadar")]
        public string NmSadar { get; set; } = string.Empty;

        [SetToNull]
        public virtual List<GeneralConsultantClinicalAssesment>? GeneralConsultantClinicalAssesments { get; set; }
    }
}