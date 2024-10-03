using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Inventory
{
    public class MaintainanceProductCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetAllMaintainanceProductQuery(Expression<Func<MaintainanceProduct, bool>>? predicate = null, bool removeCache = false) : IRequest<List<MaintainanceProductDto>>
        {
            public Expression<Func<MaintainanceProduct, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetMaintainanceProductQuery(Expression<Func<MaintainanceProduct, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<MaintainanceProductDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<MaintainanceProduct, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
        }

        public class ValidateMaintainanceProductQuery(Expression<Func<MaintainanceProduct, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<MaintainanceProduct, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateMaintainanceProductRequest(MaintainanceProductDto MaintainanceProductDto) : IRequest<MaintainanceProductDto>
        {
            public MaintainanceProductDto MaintainanceProductDto { get; set; } = MaintainanceProductDto;
        }

        public class CreateListMaintainanceProductRequest(List<MaintainanceProductDto> GeneralConsultanCPPTDtos) : IRequest<List<MaintainanceProductDto>>
        {
            public List<MaintainanceProductDto> MaintainanceProductDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateMaintainanceProductRequest(MaintainanceProductDto MaintainanceProductDto) : IRequest<MaintainanceProductDto>
        {
            public MaintainanceProductDto MaintainanceProductDto { get; set; } = MaintainanceProductDto;
        }

        public class UpdateListMaintainanceProductRequest(List<MaintainanceProductDto> MaintainanceProductDtos) : IRequest<List<MaintainanceProductDto>>
        {
            public List<MaintainanceProductDto> MaintainanceProductDtos { get; set; } = MaintainanceProductDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteMaintainanceProductRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
