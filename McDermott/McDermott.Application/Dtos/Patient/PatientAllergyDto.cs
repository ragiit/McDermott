namespace McDermott.Application.Dtos.Patient
{
    public class PatientAllergyDto : IMapFrom<PatientAllergy>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Farmacology { get; set; }
        public string? Weather { get; set; }
        public string? Food { get; set; }

        public UserDto? User { get; set; }
    }
}