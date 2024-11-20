namespace McDermott.Application.Features.Commands.Inventory
{
    public class InventoryAdjusmentDetailCommand
    {
        #region GET 

        public class GetSingleInventoryAdjusmentDetailQuery : IRequest<InventoryAdjusmentDetailDto>
        {
            public List<Expression<Func<InventoryAdjusmentDetail, object>>> Includes { get; set; }
            public Expression<Func<InventoryAdjusmentDetail, bool>> Predicate { get; set; }
            public Expression<Func<InventoryAdjusmentDetail, InventoryAdjusmentDetail>> Select { get; set; }

            public List<(Expression<Func<InventoryAdjusmentDetail, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetInventoryAdjusmentDetailQuery : IRequest<(List<InventoryAdjusmentDetailDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<InventoryAdjusmentDetail, object>>> Includes { get; set; }
            public Expression<Func<InventoryAdjusmentDetail, bool>> Predicate { get; set; }
            public Expression<Func<InventoryAdjusmentDetail, InventoryAdjusmentDetail>> Select { get; set; }

            public List<(Expression<Func<InventoryAdjusmentDetail, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        #endregion

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

        public class DeleteInventoryAdjusmentDetailRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
