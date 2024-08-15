namespace McHealthCare.Application.Features.Commands.Inventory
{
    public class InventoryAdjusmentDetailCommand
    {
        #region GET

        public class GetInventoryAdjusmentDetailQuery(Expression<Func<InventoryAdjusmentDetail, bool>>? predicate = null, bool removeCache = false) : IRequest<List<InventoryAdjusmentDetailDto>>
        {
            public Expression<Func<InventoryAdjusmentDetail, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateInventoryAdjusmentDetailRequest(InventoryAdjusmentDetailDto InventoryAdjusmentDetailDto) : IRequest<InventoryAdjusmentDetailDto>
        {
            public InventoryAdjusmentDetailDto InventoryAdjusmentDetailDto { get; set; } = InventoryAdjusmentDetailDto;
        }

        public class CreateListInventoryAdjusmentDetailRequest(List<InventoryAdjusmentDetailDto> InventoryAdjusmentDetailDtos) : IRequest<List<InventoryAdjusmentDetailDto>>
        {
            public List<InventoryAdjusmentDetailDto> InventoryAdjusmentDetailDtos { get; set; } = InventoryAdjusmentDetailDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateInventoryAdjusmentDetailRequest(InventoryAdjusmentDetailDto InventoryAdjusmentDetailDto) : IRequest<InventoryAdjusmentDetailDto>
        {
            public InventoryAdjusmentDetailDto InventoryAdjusmentDetailDto { get; set; } = InventoryAdjusmentDetailDto;
        }

        public class UpdateListInventoryAdjusmentDetailRequest(List<InventoryAdjusmentDetailDto> InventoryAdjusmentDetailDtos) : IRequest<List<InventoryAdjusmentDetailDto>>
        {
            public List<InventoryAdjusmentDetailDto> InventoryAdjusmentDetailDtos { get; set; } = InventoryAdjusmentDetailDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteInventoryAdjusmentDetailRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}