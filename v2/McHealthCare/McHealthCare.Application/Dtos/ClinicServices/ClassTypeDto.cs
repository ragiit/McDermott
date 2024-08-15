namespace McHealthCare.Application.Dtos.ClinicServices
{
    public class ClassTypeDto : IMapFrom<ClassType>
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}