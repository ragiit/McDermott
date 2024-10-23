namespace McDermott.Application.Features.Commands.Transaction
{
    public class AccidentCommand
    {
        #region GET

        public class GetSingleAccidentQuery : IRequest<AccidentDto>
        {
            public List<Expression<Func<Accident, object>>> Includes { get; set; }
            public Expression<Func<Accident, bool>> Predicate { get; set; }
            public Expression<Func<Accident, Accident>> Select { get; set; }

            public List<(Expression<Func<Accident, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetAccidentQuery : IRequest<(List<AccidentDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Accident, object>>> Includes { get; set; }
            public Expression<Func<Accident, bool>> Predicate { get; set; }
            public Expression<Func<Accident, Accident>> Select { get; set; }

            public List<(Expression<Func<Accident, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        #endregion GET

        #region CREATE

        public class CreateAccidentRequest(AccidentDto AccidentDto) : IRequest<AccidentDto>
        {
            public AccidentDto AccidentDto { get; set; } = AccidentDto;
        }

        public class CreateListAccidentRequest(List<AccidentDto> AccidentDtos) : IRequest<List<AccidentDto>>
        {
            public List<AccidentDto> AccidentDtos { get; set; } = AccidentDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateAccidentRequest(AccidentDto AccidentDto) : IRequest<AccidentDto>
        {
            public AccidentDto AccidentDto { get; set; } = AccidentDto;
        }

        public class UpdateListAccidentRequest(List<AccidentDto> AccidentDtos) : IRequest<List<AccidentDto>>
        {
            public List<AccidentDto> AccidentDtos { get; set; } = AccidentDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteAccidentRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        public class DeleteAccidentByGcIdRequest : IRequest<bool>
        {
            public long Id { get; set; }
            public List<long> Ids { get; set; }
        }

        #endregion DELETE
    }
}