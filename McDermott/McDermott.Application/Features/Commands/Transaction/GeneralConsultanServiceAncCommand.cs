namespace McDermott.Application.Features.Commands.Transaction
{
    public class GeneralConsultanServiceAncCommand
    {
        #region GET

        public class GetSingleGeneralConsultanServiceAncQuery : IRequest<GeneralConsultanServiceAncDto>
        {
            public List<Expression<Func<GeneralConsultanServiceAnc, object>>> Includes { get; set; }
            public Expression<Func<GeneralConsultanServiceAnc, bool>> Predicate { get; set; }
            public Expression<Func<GeneralConsultanServiceAnc, GeneralConsultanServiceAnc>> Select { get; set; }

            public List<(Expression<Func<GeneralConsultanServiceAnc, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetGeneralConsultanServiceAncQuery : IRequest<(List<GeneralConsultanServiceAncDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<GeneralConsultanServiceAnc, object>>> Includes { get; set; }
            public Expression<Func<GeneralConsultanServiceAnc, bool>> Predicate { get; set; }
            public Expression<Func<GeneralConsultanServiceAnc, GeneralConsultanServiceAnc>> Select { get; set; }

            public List<(Expression<Func<GeneralConsultanServiceAnc, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateGeneralConsultanServiceAnc(Expression<Func<GeneralConsultanServiceAnc, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<GeneralConsultanServiceAnc, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateGeneralConsultanServiceAncRequest(GeneralConsultanServiceAncDto GeneralConsultanServiceAncDto) : IRequest<GeneralConsultanServiceAncDto>
        {
            public GeneralConsultanServiceAncDto GeneralConsultanServiceAncDto { get; set; } = GeneralConsultanServiceAncDto;
        }

        public class BulkValidateGeneralConsultanServiceAnc(List<GeneralConsultanServiceAncDto> GeneralConsultanServiceAncsToValidate) : IRequest<List<GeneralConsultanServiceAncDto>>
        {
            public List<GeneralConsultanServiceAncDto> GeneralConsultanServiceAncsToValidate { get; } = GeneralConsultanServiceAncsToValidate;
        }

        public class CreateListGeneralConsultanServiceAncRequest(List<GeneralConsultanServiceAncDto> GeneralConsultanServiceAncDtos) : IRequest<List<GeneralConsultanServiceAncDto>>
        {
            public List<GeneralConsultanServiceAncDto> GeneralConsultanServiceAncDtos { get; set; } = GeneralConsultanServiceAncDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateGeneralConsultanServiceAncRequest(GeneralConsultanServiceAncDto GeneralConsultanServiceAncDto) : IRequest<GeneralConsultanServiceAncDto>
        {
            public GeneralConsultanServiceAncDto GeneralConsultanServiceAncDto { get; set; } = GeneralConsultanServiceAncDto;
        }

        public class UpdateListGeneralConsultanServiceAncRequest(List<GeneralConsultanServiceAncDto> GeneralConsultanServiceAncDtos) : IRequest<List<GeneralConsultanServiceAncDto>>
        {
            public List<GeneralConsultanServiceAncDto> GeneralConsultanServiceAncDtos { get; set; } = GeneralConsultanServiceAncDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteGeneralConsultanServiceAncRequest : IRequest<bool>
        {
            public long Id { get; set; }
            public List<long> Ids { get; set; }
        }

        #endregion DELETE
    }
}