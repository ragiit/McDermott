namespace McDermott.Application.Features.Commands.Medical
{
    public class CronisCategoryCommand
    {
        #region GET

        public class GetCronisCategoryQuery(Expression<Func<CronisCategory, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<CronisCategory, object>>>? includes = null, Expression<Func<CronisCategory, CronisCategory>>? select = null) : IRequest<(List<CronisCategoryDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<CronisCategory, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<CronisCategory, object>>> Includes { get; } = includes!;
            public Expression<Func<CronisCategory, CronisCategory>>? Select { get; } = select!;
        }

        public class BulkValidateCronisCategoryQuery(List<CronisCategoryDto> CronisCategorysToValidate) : IRequest<List<CronisCategoryDto>>
        {
            public List<CronisCategoryDto> CronisCategorysToValidate { get; } = CronisCategorysToValidate;
        }

        public class ValidateCronisCategoryQuery(Expression<Func<CronisCategory, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<CronisCategory, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateCronisCategoryRequest(CronisCategoryDto CronisCategoryDto) : IRequest<CronisCategoryDto>
        {
            public CronisCategoryDto CronisCategoryDto { get; set; } = CronisCategoryDto;
        }

        public class CreateListCronisCategoryRequest(List<CronisCategoryDto> CronisCategoryDtos) : IRequest<List<CronisCategoryDto>>
        {
            public List<CronisCategoryDto> CronisCategoryDtos { get; set; } = CronisCategoryDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateCronisCategoryRequest(CronisCategoryDto CronisCategoryDto) : IRequest<CronisCategoryDto>
        {
            public CronisCategoryDto CronisCategoryDto { get; set; } = CronisCategoryDto;
        }

        public class UpdateListCronisCategoryRequest(List<CronisCategoryDto> CronisCategoryDtos) : IRequest<List<CronisCategoryDto>>
        {
            public List<CronisCategoryDto> CronisCategoryDtos { get; set; } = CronisCategoryDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteCronisCategoryRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}