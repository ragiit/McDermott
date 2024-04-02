namespace McDermott.Application.Features.Commands.Inventory
{
    public class UomCategoryCommand
    {
        #region GET 

        public class GetUomCategoryQuery(Expression<Func<UomCategory, bool>>? predicate = null, bool removeCache = false) : IRequest<List<UomCategoryDto>>
        {
            public Expression<Func<UomCategory, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

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
