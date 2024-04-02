namespace McDermott.Application.Features.Commands.Inventory
{
    public class UomCommand
    {
        #region GET 

        public class GetUomQuery(Expression<Func<Uom, bool>>? predicate = null, bool removeCache = false) : IRequest<List<UomDto>>
        {
            public Expression<Func<Uom, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

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
