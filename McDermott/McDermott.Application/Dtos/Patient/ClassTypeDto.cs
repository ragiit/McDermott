namespace McDermott.Application.Dtos.Patient
{
    public class ClassTypeDto : IMapFrom<ClassType>
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}