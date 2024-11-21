namespace McDermott.Application.Features.Commands.Transaction
{
    public class GeneralConsultanCPPTCommand
    {
        #region Get

        public class GetGeneralConsultanCPPTQuery(Expression<Func<GeneralConsultanCPPT, bool>>? predicate = null, bool RemoveCache = false) : IRequest<List<GeneralConsultanCPPTDto>>
        {
            public Expression<Func<GeneralConsultanCPPT, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = RemoveCache!;
        }

        public class GetGeneralConsultanCPPTsQuery : IRequest<(List<GeneralConsultanCPPTDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<GeneralConsultanCPPT, object>>> Includes { get; set; }
            public Expression<Func<GeneralConsultanCPPT, bool>> Predicate { get; set; }
            public Expression<Func<GeneralConsultanCPPT, GeneralConsultanCPPT>> Select { get; set; }

            public List<(Expression<Func<GeneralConsultanCPPT, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetSingleGeneralConsultanCPPTsQuery : IRequest<GeneralConsultanCPPTDto>
        {
            public List<Expression<Func<GeneralConsultanCPPT, object>>> Includes { get; set; }
            public Expression<Func<GeneralConsultanCPPT, bool>> Predicate { get; set; }
            public Expression<Func<GeneralConsultanCPPT, GeneralConsultanCPPT>> Select { get; set; }

            public List<(Expression<Func<GeneralConsultanCPPT, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        #endregion Get

        #region Create

        public class CreateGeneralConsultanCPPTRequest(GeneralConsultanCPPTDto GeneralConsultanCPPTDto) : IRequest<GeneralConsultanCPPTDto>
        {
            public GeneralConsultanCPPTDto GeneralConsultanCPPTDto { get; set; } = GeneralConsultanCPPTDto;
        }

        public class CreateListGeneralConsultanCPPTRequest(List<GeneralConsultanCPPTDto> GeneralConsultanCPPTDtos) : IRequest<List<GeneralConsultanCPPTDto>>
        {
            public List<GeneralConsultanCPPTDto> GeneralConsultanCPPTDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion Create

        #region Update

        public class UpdateGeneralConsultanCPPTRequest(GeneralConsultanCPPTDto GeneralConsultanCPPTDto) : IRequest<GeneralConsultanCPPTDto>
        {
            public GeneralConsultanCPPTDto GeneralConsultanCPPTDto { get; set; } = GeneralConsultanCPPTDto;
        }

        public class UpdateListGeneralConsultanCPPTRequest(List<GeneralConsultanCPPTDto> GeneralConsultanCPPTDtos) : IRequest<List<GeneralConsultanCPPTDto>>
        {
            public List<GeneralConsultanCPPTDto> GeneralConsultanCPPTDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion Update

        #region Delete

        public class DeleteGeneralConsultanCPPTRequest(long? id = null, List<long>? ids = null, long? deleteByGeneralServiceId = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public long DeleteByGeneralServiceId { get; set; } = deleteByGeneralServiceId ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion Delete
    }
}