using McDermott.Application.Dtos.AwarenessEvent;

namespace McDermott.Application.Features.Commands.AwarenessEvent
{
    public class AwarenessEduCategoryCommand
    {
        #region GET Education Program Detail

        public class GetAllAwarenessEduCategoryQuery(Expression<Func<AwarenessEduCategory, bool>>? predicate = null, bool removeCache = false) : IRequest<List<AwarenessEduCategoryDto>>
        {
            public Expression<Func<AwarenessEduCategory, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetSingleAwarenessEduCategoryQuery : IRequest<AwarenessEduCategoryDto>
        {
            public List<Expression<Func<AwarenessEduCategory, object>>> Includes { get; set; }
            public Expression<Func<AwarenessEduCategory, bool>> Predicate { get; set; }
            public Expression<Func<AwarenessEduCategory, AwarenessEduCategory>> Select { get; set; }

            public List<(Expression<Func<AwarenessEduCategory, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetAwarenessEduCategoryQuery : IRequest<(List<AwarenessEduCategoryDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<AwarenessEduCategory, object>>> Includes { get; set; }
            public Expression<Func<AwarenessEduCategory, bool>> Predicate { get; set; }
            public Expression<Func<AwarenessEduCategory, AwarenessEduCategory>> Select { get; set; }

            public List<(Expression<Func<AwarenessEduCategory, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateAwarenessEduCategoryQuery(List<AwarenessEduCategoryDto> AwarenessEduCategoryToValidate) : IRequest<List<AwarenessEduCategoryDto>>
        {
            public List<AwarenessEduCategoryDto> AwarenessEduCategoryToValidate { get; } = AwarenessEduCategoryToValidate;
        }

        public class ValidateAwarenessEduCategoryQuery(Expression<Func<AwarenessEduCategory, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<AwarenessEduCategory, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET Education Program Detail

        #region CREATE Education Program

        public class CreateAwarenessEduCategoryRequest(AwarenessEduCategoryDto AwarenessEduCategoryDto) : IRequest<AwarenessEduCategoryDto>
        {
            public AwarenessEduCategoryDto AwarenessEduCategoryDto { get; set; } = AwarenessEduCategoryDto;
        }

        public class CreateListAwarenessEduCategoryRequest(List<AwarenessEduCategoryDto> AwarenessEduCategoryDtos) : IRequest<List<AwarenessEduCategoryDto>>
        {
            public List<AwarenessEduCategoryDto> AwarenessEduCategoryDtos { get; set; } = AwarenessEduCategoryDtos;
        }

        #endregion CREATE Education Program

        #region UPDATE Education Program

        public class UpdateAwarenessEduCategoryRequest(AwarenessEduCategoryDto AwarenessEduCategoryDto) : IRequest<AwarenessEduCategoryDto>
        {
            public AwarenessEduCategoryDto AwarenessEduCategoryDto { get; set; } = AwarenessEduCategoryDto;
        }

        public class UpdateListAwarenessEduCategoryRequest(List<AwarenessEduCategoryDto> AwarenessEduCategoryDtos) : IRequest<List<AwarenessEduCategoryDto>>
        {
            public List<AwarenessEduCategoryDto> AwarenessEduCategoryDtos { get; set; } = AwarenessEduCategoryDtos;
        }

        #endregion UPDATE Education Program

        #region DELETE Education Program

        public class DeleteAwarenessEduCategoryRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Education Program
    }
}