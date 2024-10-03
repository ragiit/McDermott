namespace McDermott.Application.Features.Commands.Inventory
{
    public class UomCommand
    {
        #region GET

        public class GetAllUomQuery(Expression<Func<Uom, bool>>? predicate = null, bool removeCache = false) : IRequest<List<UomDto>>
        {
            public Expression<Func<Uom, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetUomQuery(Expression<Func<Uom, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<Uom, object>>>? includes = null, Expression<Func<Uom, Uom>>? select = null) : IRequest<(List<UomDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Uom, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<Uom, object>>> Includes { get; } = includes!;
            public Expression<Func<Uom, Uom>>? Select { get; } = select!;
        }

        public class BulkValidateUomQuery(List<UomDto> UomToValidate) : IRequest<List<UomDto>>
        {
            public List<UomDto> UomToValidate { get; } = UomToValidate;
        }

        public class ValidateUomQuery(Expression<Func<Uom, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Uom, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateUomRequest(UomDto UomDto) : IRequest<UomDto>
        {
            public UomDto UomDto { get; set; } = UomDto;
        }

        public class CreateListUomRequest(List<UomDto> UomDtos) : IRequest<List<UomDto>>
        {
            public List<UomDto> UomDtos { get; set; } = UomDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateUomRequest(UomDto UomDto) : IRequest<UomDto>
        {
            public UomDto UomDto { get; set; } = UomDto;
        }

        public class UpdateListUomRequest(List<UomDto> UomDtos) : IRequest<List<UomDto>>
        {
            public List<UomDto> UomDtos { get; set; } = UomDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteUomRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}