namespace McDermott.Application.Dtos.Config
{
    public class GenderDto : IMapFrom<Gender>
    {
         public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }
}