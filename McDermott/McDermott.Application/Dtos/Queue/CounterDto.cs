using FluentValidation;

namespace McDermott.Application.Dtos.Queue
{
    public class CounterDto : IMapFrom<Counter>
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsActive { get; set; }
        public long? ServiceId { get; set; }
        public long? ServiceKId { get; set; }
        public long? PhysicianId { get; set; }

        public string? Status { get; set; } = string.Empty;

        public virtual ServiceDto? Service { get; set; }
    }

    public class AddCounterPopUp : AbstractValidator<CounterDto>
    {
        public AddCounterPopUp()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("The Name field is required");
        }
    }
}