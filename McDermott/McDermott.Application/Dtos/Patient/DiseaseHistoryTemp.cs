namespace McDermott.Application.Dtos.Patient
{
    public class DiseaseHistoryTemp
    {
        public long Id { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string Patient { get; set; } = string.Empty;
        public string Disease { get; set; } = string.Empty;
        public string CronisCategory { get; set; } = string.Empty;
        public string Physician { get; set; } = string.Empty;
        public DateTime? DiseaseDate { get; set; }
    }
}