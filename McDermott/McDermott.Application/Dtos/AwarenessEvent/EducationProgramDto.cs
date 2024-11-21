using System.Diagnostics.CodeAnalysis;

namespace McDermott.Application.Dtos.AwarenessEvent
{
    public class EducationProgramDto : IMapFrom<EducationProgram>
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Event Name is required.")]
        public string? EventName { get; set; }

        [Required(ErrorMessage = "Event Category is required.")]
        public long? EventCategoryId { get; set; }

        public string? HTMLContent { get; set; }

        public string? Description { get; set; }

        public string? HTMLMaterial { get; set; }

        public string? EventLink { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }

        public string? Slug { get; set; }
        public string? Attendance { get; set; }
        public EnumStatusEducationProgram? Status { get; set; }

        [NotNull]
        public AwarenessEduCategoryDto? EventCategory { get; set; }
    }
}