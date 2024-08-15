namespace McHealthCare.Application.Features.Commands.Inventory
{
    public class UomCommand
    {
        #region GET

        public class GetUomQuery(Expression<Func<Uom, bool>>? predicate = null, bool removeCache = false) : IRequest<List<UomDto>>
        {
            public Expression<Func<Uom, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
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

        public class DeleteUomRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}