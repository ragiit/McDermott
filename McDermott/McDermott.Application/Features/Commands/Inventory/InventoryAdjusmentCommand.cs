namespace McDermott.Application.Features.Commands.Inventory
{
    public class InventoryAdjusmentCommand
    {
        #region GET

        public class GetSingleInventoryAdjusmentQuery : IRequest<InventoryAdjusmentDto>
        {
            public List<Expression<Func<InventoryAdjusment, object>>> Includes { get; set; }
            public Expression<Func<InventoryAdjusment, bool>> Predicate { get; set; }
            public Expression<Func<InventoryAdjusment, InventoryAdjusment>> Select { get; set; }

            public List<(Expression<Func<InventoryAdjusment, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetInventoryAdjusmentQuery : IRequest<(List<InventoryAdjusmentDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<InventoryAdjusment, object>>> Includes { get; set; }
            public Expression<Func<InventoryAdjusment, bool>> Predicate { get; set; }
            public Expression<Func<InventoryAdjusment, InventoryAdjusment>> Select { get; set; }

            public List<(Expression<Func<InventoryAdjusment, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetInventoryAdjusmentLogQuery(Expression<Func<InventoryAdjusmentLog, bool>>? predicate = null, bool removeCache = false) : IRequest<List<InventoryAdjustmentLogDto>>
        {
            public Expression<Func<InventoryAdjusmentLog, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

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