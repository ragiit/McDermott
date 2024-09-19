using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Inventory
{
    public class MaintinanceRecordCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)
        public class GetAllMaintainanceRecordQuery(Expression<Func<MaintainanceRecord, bool>>? predicate = null, bool removeCache = false) : IRequest<List<MaintainanceRecordDto>>
        {
            public Expression<Func<MaintainanceRecord, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        public class GetMaintainanceRecordQuery(Expression<Func<MaintainanceRecord, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<MaintainanceRecordDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<MaintainanceRecord, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
        }

        public class ValidateMaintainanceRecordQuery(Expression<Func<MaintainanceRecord, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<MaintainanceRecord, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateMaintainanceRecordRequest(MaintainanceRecordDto MaintainanceRecordDto) : IRequest<MaintainanceRecordDto>
        {
            public MaintainanceRecordDto MaintainanceRecordDto { get; set; } = MaintainanceRecordDto;
        }

        public class CreateListMaintainanceRecordRequest(List<MaintainanceRecordDto> GeneralConsultanCPPTDtos) : IRequest<List<MaintainanceRecordDto>>
        {
            public List<MaintainanceRecordDto> MaintainanceRecordDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateMaintainanceRecordRequest(MaintainanceRecordDto MaintainanceRecordDto) : IRequest<MaintainanceRecordDto>
        {
            public MaintainanceRecordDto MaintainanceRecordDto { get; set; } = MaintainanceRecordDto;
        }

        public class UpdateListMaintainanceRecordRequest(List<MaintainanceRecordDto> MaintainanceRecordDtos) : IRequest<List<MaintainanceRecordDto>>
        {
            public List<MaintainanceRecordDto> MaintainanceRecordDtos { get; set; } = MaintainanceRecordDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteMaintainanceRecordRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
