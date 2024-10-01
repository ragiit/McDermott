namespace McDermott.Application.Features.Commands.Config
{
    public class OccupationalCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetOccupationalQuery(Expression<Func<Occupational, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<Occupational, object>>>? includes = null, Expression<Func<Occupational, Occupational>>? select = null) : IRequest<(List<OccupationalDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Occupational, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<Occupational, object>>> Includes { get; } = includes!;
            public Expression<Func<Occupational, Occupational>>? Select { get; } = select!;
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