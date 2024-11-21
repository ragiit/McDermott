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

        public class GetSingleUomQuery : IRequest<UomDto>
        {
            public List<Expression<Func<Uom, object>>> Includes { get; set; }
            public Expression<Func<Uom, bool>> Predicate { get; set; }
            public Expression<Func<Uom, Uom>> Select { get; set; }

            public List<(Expression<Func<Uom, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetUomQuery : IRequest<(List<UomDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Uom, object>>> Includes { get; set; }
            public Expression<Func<Uom, bool>> Predicate { get; set; }
            public Expression<Func<Uom, Uom>> Select { get; set; }

            public List<(Expression<Func<Uom, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateUomQuery(List<UomDto> UomToValidate) : IRequest<List<UomDto>>
        {
            public List<UomDto> UomToValidate { get; } = UomToValidate;
        }

        public class BulkValidateUomNameTypeQuery(List<UomDto> UomToValidate) : IRequest<List<UomDto>>
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