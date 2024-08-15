namespace McHealthCare.Application.Features.CommandsQueries.Inventory
{
    public class UomCategoryCommand
    {
        #region GET

        public class GetUomCategoryQuery(Expression<Func<UomCategory, bool>>? predicate = null, bool removeCache = false) : IRequest<List<UomCategoryDto>>
        {
            public Expression<Func<UomCategory, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
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

        public class DeleteUomCategoryRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}