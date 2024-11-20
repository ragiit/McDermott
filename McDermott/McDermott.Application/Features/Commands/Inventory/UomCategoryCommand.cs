using McDermott.Application.Dtos.Pharmacies;

namespace McDermott.Application.Features.Commands.Inventory
{
    public class UomCategoryCommand
    {
        #region GET

        public class GetAllUomCategoryQuery(Expression<Func<UomCategory, bool>>? predicate = null, bool removeCache = false) : IRequest<List<UomCategoryDto>>
        {
            public Expression<Func<UomCategory, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetUomCategoryQuery(Expression<Func<UomCategory, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<UomCategory, object>>>? includes = null, Expression<Func<UomCategory, UomCategory>>? select = null) : IRequest<(List<UomCategoryDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<UomCategory, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<UomCategory, object>>> Includes { get; } = includes!;
            public Expression<Func<UomCategory, UomCategory>>? Select { get; } = select!;
        }

        public class BulkValidateUomCategoryQuery(List<UomCategoryDto> UomCategoryToValidate) : IRequest<List<UomCategoryDto>>
        {
            public List<UomCategoryDto> UomCategoryToValidate { get; } = UomCategoryToValidate;
        }

        public class ValidateUomCategoryQuery(Expression<Func<UomCategory, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<UomCategory, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateUomCategoryRequest(UomCategoryDto UomCategoryDto) : IRequest<UomCategoryDto>
        {
            public UomCategoryDto UomCategoryDto { get; set; } = UomCategoryDto;
        }

        public class CreateListUomCategoryRequest(List<UomCategoryDto> UomCategoryDtos) : IRequest<List<UomCategoryDto>>
        {
            public List<UomCategoryDto> UomCategoryDtos { get; set; } = UomCategoryDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateUomCategoryRequest(UomCategoryDto UomCategoryDto) : IRequest<UomCategoryDto>
        {
            public UomCategoryDto UomCategoryDto { get; set; } = UomCategoryDto;
        }

        public class UpdateListUomCategoryRequest(List<UomCategoryDto> UomCategoryDtos) : IRequest<List<UomCategoryDto>>
        {
            public List<UomCategoryDto> UomCategoryDtos { get; set; } = UomCategoryDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteUomCategoryRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}