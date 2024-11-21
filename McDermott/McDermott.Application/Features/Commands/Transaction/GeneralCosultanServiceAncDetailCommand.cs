namespace McDermott.Application.Features.Commands.Transaction
{
    public class GeneralConsultanServiceAncDetailCommand
    {
        #region GET

        public class GetSingleGeneralConsultanServiceAncDetailQuery : IRequest<GeneralConsultanServiceAncDetailDto>
        {
            public List<Expression<Func<GeneralConsultanServiceAncDetail, object>>> Includes { get; set; }
            public Expression<Func<GeneralConsultanServiceAncDetail, bool>> Predicate { get; set; }
            public Expression<Func<GeneralConsultanServiceAncDetail, GeneralConsultanServiceAncDetail>> Select { get; set; }

            public List<(Expression<Func<GeneralConsultanServiceAncDetail, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetGeneralConsultanServiceAncDetailQuery : IRequest<(List<GeneralConsultanServiceAncDetailDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<GeneralConsultanServiceAncDetail, object>>> Includes { get; set; }
            public Expression<Func<GeneralConsultanServiceAncDetail, bool>> Predicate { get; set; }
            public Expression<Func<GeneralConsultanServiceAncDetail, GeneralConsultanServiceAncDetail>> Select { get; set; }

            public List<(Expression<Func<GeneralConsultanServiceAncDetail, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateGeneralConsultanServiceAncDetail(Expression<Func<GeneralConsultanServiceAncDetail, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<GeneralConsultanServiceAncDetail, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateGeneralConsultanServiceAncDetailRequest(GeneralConsultanServiceAncDetailDto GeneralConsultanServiceAncDetailDto) : IRequest<GeneralConsultanServiceAncDetailDto>
        {
            public GeneralConsultanServiceAncDetailDto GeneralConsultanServiceAncDetailDto { get; set; } = GeneralConsultanServiceAncDetailDto;
        }

        public class BulkValidateGeneralConsultanServiceAncDetail(List<GeneralConsultanServiceAncDetailDto> GeneralConsultanServiceAncDetailsToValidate) : IRequest<List<GeneralConsultanServiceAncDetailDto>>
        {
            public List<GeneralConsultanServiceAncDetailDto> GeneralConsultanServiceAncDetailsToValidate { get; } = GeneralConsultanServiceAncDetailsToValidate;
        }

        public class CreateListGeneralConsultanServiceAncDetailRequest(List<GeneralConsultanServiceAncDetailDto> GeneralConsultanServiceAncDetailDtos) : IRequest<List<GeneralConsultanServiceAncDetailDto>>
        {
            public List<GeneralConsultanServiceAncDetailDto> GeneralConsultanServiceAncDetailDtos { get; set; } = GeneralConsultanServiceAncDetailDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateGeneralConsultanServiceAncDetailRequest(GeneralConsultanServiceAncDetailDto GeneralConsultanServiceAncDetailDto) : IRequest<GeneralConsultanServiceAncDetailDto>
        {
            public GeneralConsultanServiceAncDetailDto GeneralConsultanServiceAncDetailDto { get; set; } = GeneralConsultanServiceAncDetailDto;
        }

        public class UpdateListGeneralConsultanServiceAncDetailRequest(List<GeneralConsultanServiceAncDetailDto> GeneralConsultanServiceAncDetailDtos) : IRequest<List<GeneralConsultanServiceAncDetailDto>>
        {
            public List<GeneralConsultanServiceAncDetailDto> GeneralConsultanServiceAncDetailDtos { get; set; } = GeneralConsultanServiceAncDetailDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteGeneralConsultanServiceAncDetailRequest : IRequest<bool>
        {
            public long Id { get; set; }
            public List<long> Ids { get; set; }
        }

        #endregion DELETE
    }
}