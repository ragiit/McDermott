using McDermott.Application.Dtos.Queue;

namespace McDermott.Application.Features.Commands.Queue
{
    public class CounterCommand
    {
        public class GetCounterQuery : IRequest<List<CounterDto>>;

        public class GetCounterByIdQuery : IRequest<CounterDto>
        {
            public int Id { get; set; }

            public GetCounterByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateCounterRequest : IRequest<CounterDto>
        {
            public CounterDto CounterDto { get; set; }

            public CreateCounterRequest(CounterDto CounterDto)
            {
                this.CounterDto = CounterDto;
            }
        }

        public class UpdateCounterRequest : IRequest<bool>
        {
            public CounterDto CounterDto { get; set; }

            public UpdateCounterRequest(CounterDto CounterDto)
            {
                this.CounterDto = CounterDto;
            }
        }

        public class DeleteCounterRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteCounterRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListCounterRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListCounterRequest(List<int> id)
            {
                Id = id;
            }
        }
    }
}