using McDermott.Application.Dtos.Inventory;

namespace McDermott.Application.Features.Commands.Inventory
{
    public class InventoryAdjusmentCommand
    {
        #region GET 

        public class GetInventoryAdjusmentQuery(Expression<Func<InventoryAdjusment, bool>>? predicate = null, bool removeCache = false) : IRequest<List<InventoryAdjusmentDto>>
        {
            public Expression<Func<InventoryAdjusment, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        } 
        public class GetInventoryAdjusmentLogQuery(Expression<Func<InventoryAdjusmentLog, bool>>? predicate = null, bool removeCache = false) : IRequest<List<InventoryAdjustmentLogDto>>
        {
            public Expression<Func<InventoryAdjusmentLog, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion

        #region CREATE

        public class CreateInventoryAdjusmentRequest(InventoryAdjusmentDto InventoryAdjusmentDto) : IRequest<InventoryAdjusmentDto>
        {
            public InventoryAdjusmentDto InventoryAdjusmentDto { get; set; } = InventoryAdjusmentDto;
        }

        public class CreateListInventoryAdjusmentRequest(List<InventoryAdjusmentDto> InventoryAdjusmentDtos) : IRequest<List<InventoryAdjusmentDto>>
        {
            public List<InventoryAdjusmentDto> InventoryAdjusmentDtos { get; set; } = InventoryAdjusmentDtos;
        }

        public class CreateInventoryAdjusmentLogRequest(InventoryAdjustmentLogDto InventoryAdjusmentLogDto) : IRequest<InventoryAdjustmentLogDto>
        {
            public InventoryAdjustmentLogDto InventoryAdjusmentLogDto { get; set; } = InventoryAdjusmentLogDto;
        }

        public class CreateListInventoryAdjusmentLogRequest(List<InventoryAdjustmentLogDto> InventoryAdjusmentLogDtos) : IRequest<List<InventoryAdjustmentLogDto>>
        {
            public List<InventoryAdjustmentLogDto> InventoryAdjusmentLogDtos { get; set; } = InventoryAdjusmentLogDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateInventoryAdjusmentRequest(InventoryAdjusmentDto InventoryAdjusmentDto) : IRequest<InventoryAdjusmentDto>
        {
            public InventoryAdjusmentDto InventoryAdjusmentDto { get; set; } = InventoryAdjusmentDto;
        }

        public class UpdateListInventoryAdjusmentRequest(List<InventoryAdjusmentDto> InventoryAdjusmentDtos) : IRequest<List<InventoryAdjusmentDto>>
        {
            public List<InventoryAdjusmentDto> InventoryAdjusmentDtos { get; set; } = InventoryAdjusmentDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteInventoryAdjusmentRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
