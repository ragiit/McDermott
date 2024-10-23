using McDermott.Application.Dtos.Queue;

namespace McDermott.Application.Features.Commands.Queue
{
    public class CounterCommand
    {
        #region GET

        public class GetCounterQuery : IRequest<(List<CounterDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Counter, object>>> Includes { get; set; }
            public Expression<Func<Counter, bool>> Predicate { get; set; }
            public Expression<Func<Counter, Counter>> Select { get; set; }

            public List<(Expression<Func<Counter, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetSingleCounterQuery : IRequest<CounterDto>
        {
            public List<Expression<Func<Counter, object>>> Includes { get; set; }
            public Expression<Func<Counter, bool>> Predicate { get; set; }
            public Expression<Func<Counter, Counter>> Select { get; set; }

            public List<(Expression<Func<Counter, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateCounterQuery(Expression<Func<Counter, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Counter, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateCounterRequest(CounterDto CounterDto) : IRequest<CounterDto>
        {
            public CounterDto CounterDto { get; set; } = CounterDto;
        }

        public class BulkValidateCounterQuery(List<CounterDto> CountersToValidate) : IRequest<List<CounterDto>>
        {
            public List<CounterDto> CountersToValidate { get; } = CountersToValidate;
        }

        public class CreateListCounterRequest(List<CounterDto> CounterDtos) : IRequest<List<CounterDto>>
        {
            public List<CounterDto> CounterDtos { get; set; } = CounterDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateCounterRequest(CounterDto CounterDto) : IRequest<CounterDto>
        {
            public CounterDto CounterDto { get; set; } = CounterDto;
        }

        public class UpdateListCounterRequest(List<CounterDto> CounterDtos) : IRequest<List<CounterDto>>
        {
            public List<CounterDto> CounterDtos { get; set; } = CounterDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteCounterRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}