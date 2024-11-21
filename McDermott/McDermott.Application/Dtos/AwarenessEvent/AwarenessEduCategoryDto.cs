namespace McDermott.Application.Dtos.AwarenessEvent
{
    public class AwarenessEduCategoryDto : IMapFrom<AwarenessEduCategory>
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        public string Name { get; set; } = string.Empty;
    }
}