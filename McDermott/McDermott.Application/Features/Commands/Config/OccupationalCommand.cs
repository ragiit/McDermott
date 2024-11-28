namespace McDermott.Application.Features.Commands.Config
{
    public class OccupationalCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetSingleOccupationalQuery : IRequest<OccupationalDto>
        {
            public List<Expression<Func<Occupational, object>>> Includes { get; set; }
            public Expression<Func<Occupational, bool>> Predicate { get; set; }
            public Expression<Func<Occupational, Occupational>> Select { get; set; }

            public List<(Expression<Func<Occupational, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetOccupationalQuery : IRequest<(List<OccupationalDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Occupational, object>>> Includes { get; set; }
            public Expression<Func<Occupational, bool>> Predicate { get; set; }
            public Expression<Func<Occupational, Occupational>> Select { get; set; }

            public List<(Expression<Func<Occupational, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateOccupationalQuery(List<OccupationalDto> OccupationalsToValidate) : IRequest<List<OccupationalDto>>
        {
            public List<OccupationalDto> OccupationalsToValidate { get; } = OccupationalsToValidate;
        }

        public class ValidateOccupationalQuery(Expression<Func<Occupational, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Occupational, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateOccupationalRequest(OccupationalDto OccupationalDto) : IRequest<OccupationalDto>
        {
            public OccupationalDto OccupationalDto { get; set; } = OccupationalDto;
        }

        public class CreateListOccupationalRequest(List<OccupationalDto> GeneralConsultanCPPTDtos) : IRequest<List<OccupationalDto>>
        {
            public List<OccupationalDto> OccupationalDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateOccupationalRequest(OccupationalDto OccupationalDto) : IRequest<OccupationalDto>
        {
            public OccupationalDto OccupationalDto { get; set; } = OccupationalDto;
        }

        public class UpdateListOccupationalRequest(List<OccupationalDto> OccupationalDtos) : IRequest<List<OccupationalDto>>
        {
            public List<OccupationalDto> OccupationalDtos { get; set; } = OccupationalDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteOccupationalRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}