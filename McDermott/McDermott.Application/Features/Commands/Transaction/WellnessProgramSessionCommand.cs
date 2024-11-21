namespace McDermott.Application.Features.Commands.Transaction
{
    public class WellnessProgramSessionCommand
    {
        #region GET

        public class GetSingleWellnessProgramSessionQuery : IRequest<WellnessProgramSessionDto>
        {
            public List<Expression<Func<WellnessProgramSession, object>>> Includes { get; set; }
            public Expression<Func<WellnessProgramSession, bool>> Predicate { get; set; }
            public Expression<Func<WellnessProgramSession, WellnessProgramSession>> Select { get; set; }

            public List<(Expression<Func<WellnessProgramSession, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetWellnessProgramSessionQuery : IRequest<(List<WellnessProgramSessionDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<WellnessProgramSession, object>>> Includes { get; set; }
            public Expression<Func<WellnessProgramSession, bool>> Predicate { get; set; }
            public Expression<Func<WellnessProgramSession, WellnessProgramSession>> Select { get; set; }

            public List<(Expression<Func<WellnessProgramSession, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateWellnessProgramSession(Expression<Func<WellnessProgramSession, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<WellnessProgramSession, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateWellnessProgramSessionRequest(WellnessProgramSessionDto WellnessProgramSessionDto) : IRequest<WellnessProgramSessionDto>
        {
            public WellnessProgramSessionDto WellnessProgramSessionDto { get; set; } = WellnessProgramSessionDto;
        }

        public class BulkValidateWellnessProgramSession(List<WellnessProgramSessionDto> WellnessProgramSessionsToValidate) : IRequest<List<WellnessProgramSessionDto>>
        {
            public List<WellnessProgramSessionDto> WellnessProgramSessionsToValidate { get; } = WellnessProgramSessionsToValidate;
        }

        public class CreateListWellnessProgramSessionRequest(List<WellnessProgramSessionDto> WellnessProgramSessionDtos) : IRequest<List<WellnessProgramSessionDto>>
        {
            public List<WellnessProgramSessionDto> WellnessProgramSessionDtos { get; set; } = WellnessProgramSessionDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateWellnessProgramSessionRequest(WellnessProgramSessionDto WellnessProgramSessionDto) : IRequest<WellnessProgramSessionDto>
        {
            public WellnessProgramSessionDto WellnessProgramSessionDto { get; set; } = WellnessProgramSessionDto;
        }

        public class UpdateListWellnessProgramSessionRequest(List<WellnessProgramSessionDto> WellnessProgramSessionDtos) : IRequest<List<WellnessProgramSessionDto>>
        {
            public List<WellnessProgramSessionDto> WellnessProgramSessionDtos { get; set; } = WellnessProgramSessionDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteWellnessProgramSessionRequest : IRequest<bool>
        {
            public long Id { get; set; }
            public List<long> Ids { get; set; }
        }

        #endregion DELETE
    }
}