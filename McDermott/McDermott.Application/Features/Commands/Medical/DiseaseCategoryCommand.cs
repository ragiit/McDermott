namespace McDermott.Application.Features.Commands.Medical
{
    public class DiseaseCategoryCommand
    {
        #region GET

        public class GetDiseaseCategoryQuery(Expression<Func<DiseaseCategory, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<DiseaseCategory, object>>>? includes = null, Expression<Func<DiseaseCategory, DiseaseCategory>>? select = null) : IRequest<(List<DiseaseCategoryDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<DiseaseCategory, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<DiseaseCategory, object>>> Includes { get; } = includes!;
            public Expression<Func<DiseaseCategory, DiseaseCategory>>? Select { get; } = select!;
        }

        public class BulkValidateDiseaseCategoryQuery(List<DiseaseCategoryDto> DiseaseCategorysToValidate) : IRequest<List<DiseaseCategoryDto>>
        {
            public List<DiseaseCategoryDto> DiseaseCategorysToValidate { get; } = DiseaseCategorysToValidate;
        }

        public class ValidateDiseaseCategoryQuery(Expression<Func<DiseaseCategory, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<DiseaseCategory, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateDiseaseCategoryRequest(DiseaseCategoryDto DiseaseCategoryDto) : IRequest<DiseaseCategoryDto>
        {
            public DiseaseCategoryDto DiseaseCategoryDto { get; set; } = DiseaseCategoryDto;
        }

        public class CreateListDiseaseCategoryRequest(List<DiseaseCategoryDto> DiseaseCategoryDtos) : IRequest<List<DiseaseCategoryDto>>
        {
            public List<DiseaseCategoryDto> DiseaseCategoryDtos { get; set; } = DiseaseCategoryDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateDiseaseCategoryRequest(DiseaseCategoryDto DiseaseCategoryDto) : IRequest<DiseaseCategoryDto>
        {
            public DiseaseCategoryDto DiseaseCategoryDto { get; set; } = DiseaseCategoryDto;
        }

        public class UpdateListDiseaseCategoryRequest(List<DiseaseCategoryDto> DiseaseCategoryDtos) : IRequest<List<DiseaseCategoryDto>>
        {
            public List<DiseaseCategoryDto> DiseaseCategoryDtos { get; set; } = DiseaseCategoryDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteDiseaseCategoryRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}