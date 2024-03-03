namespace McDermott.Application.Dtos.Medical
{
    public class SpecialityDto : IMapFrom<Speciality>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;
    }
}