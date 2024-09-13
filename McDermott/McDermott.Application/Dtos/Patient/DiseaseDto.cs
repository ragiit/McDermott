namespace McDermott.Application.Dtos.Patient
{
    public class DiseaseDto
    {
        public string? PatientName { get; set; }
        public string? PhycisianName { get; set; }
        public DateTime? DateDisease { get; set; }
        public string? DiseaseName { get; set; }
    }
}