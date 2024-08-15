namespace McHealthCare.Application.Dtos.ClinicServices
{
    public class ClassTypeDto : IMapFrom<ClassType>
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}