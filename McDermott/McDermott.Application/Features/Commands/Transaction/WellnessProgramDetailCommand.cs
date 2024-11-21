namespace McDermott.Application.Features.Commands.Transaction
{
    public class WellnessProgramDetailCommand
    {
        #region GET

        public class GetSingleWellnessProgramDetailQuery : IRequest<WellnessProgramDetailDto>
        {
            public List<Expression<Func<WellnessProgramDetail, object>>> Includes { get; set; }
            public Expression<Func<WellnessProgramDetail, bool>> Predicate { get; set; }
            public Expression<Func<WellnessProgramDetail, WellnessProgramDetail>> Select { get; set; }

            public List<(Expression<Func<WellnessProgramDetail, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetWellnessProgramDetailQuery : IRequest<(List<WellnessProgramDetailDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<WellnessProgramDetail, object>>> Includes { get; set; }
            public Expression<Func<WellnessProgramDetail, bool>> Predicate { get; set; }
            public Expression<Func<WellnessProgramDetail, WellnessProgramDetail>> Select { get; set; }

            public List<(Expression<Func<WellnessProgramDetail, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateWellnessProgramDetail(Expression<Func<WellnessProgramDetail, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<WellnessProgramDetail, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateWellnessProgramDetailRequest(WellnessProgramDetailDto WellnessProgramDetailDto) : IRequest<WellnessProgramDetailDto>
        {
            public WellnessProgramDetailDto WellnessProgramDetailDto { get; set; } = WellnessProgramDetailDto;
        }

        public class BulkValidateWellnessProgramDetail(List<WellnessProgramDetailDto> WellnessProgramDetailsToValidate) : IRequest<List<WellnessProgramDetailDto>>
        {
            public List<WellnessProgramDetailDto> WellnessProgramDetailsToValidate { get; } = WellnessProgramDetailsToValidate;
        }

        public class CreateListWellnessProgramDetailRequest(List<WellnessProgramDetailDto> WellnessProgramDetailDtos) : IRequest<List<WellnessProgramDetailDto>>
        {
            public List<WellnessProgramDetailDto> WellnessProgramDetailDtos { get; set; } = WellnessProgramDetailDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateWellnessProgramDetailRequest(WellnessProgramDetailDto WellnessProgramDetailDto) : IRequest<WellnessProgramDetailDto>
        {
            public WellnessProgramDetailDto WellnessProgramDetailDto { get; set; } = WellnessProgramDetailDto;
        }

        public class UpdateListWellnessProgramDetailRequest(List<WellnessProgramDetailDto> WellnessProgramDetailDtos) : IRequest<List<WellnessProgramDetailDto>>
        {
            public List<WellnessProgramDetailDto> WellnessProgramDetailDtos { get; set; } = WellnessProgramDetailDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteWellnessProgramDetailRequest : IRequest<bool>
        {
            public long Id { get; set; }
            public List<long> Ids { get; set; }
        }

        #endregion DELETE
    }
}