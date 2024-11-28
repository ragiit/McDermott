namespace McDermott.Application.Features.Commands.Medical
{
    public class CronisCategoryCommand
    {
        #region GET

        public class GetSingleCronisCategoryQuery : IRequest<CronisCategoryDto>
        {
            public List<Expression<Func<CronisCategory, object>>> Includes { get; set; }
            public Expression<Func<CronisCategory, bool>> Predicate { get; set; }
            public Expression<Func<CronisCategory, CronisCategory>> Select { get; set; }

            public List<(Expression<Func<CronisCategory, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetCronisCategoryQuery : IRequest<(List<CronisCategoryDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<CronisCategory, object>>> Includes { get; set; }
            public Expression<Func<CronisCategory, bool>> Predicate { get; set; }
            public Expression<Func<CronisCategory, CronisCategory>> Select { get; set; }

            public List<(Expression<Func<CronisCategory, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
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