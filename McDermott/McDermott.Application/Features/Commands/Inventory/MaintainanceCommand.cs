using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Inventory
{
    public class MaintainanceCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetAllMaintainanceQuery(Expression<Func<Maintainance, bool>>? predicate = null, bool removeCache = false) : IRequest<List<MaintainanceDto>>
        {
            public Expression<Func<Maintainance, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetMaintainanceQuery(Expression<Func<Maintainance, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<MaintainanceDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Maintainance, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
        }

        public class ValidateMaintainanceQuery(Expression<Func<Maintainance, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Maintainance, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateMaintainanceRequest(MaintainanceDto MaintainanceDto) : IRequest<MaintainanceDto>
        {
            public MaintainanceDto MaintainanceDto { get; set; } = MaintainanceDto;
        }

        public class CreateListMaintainanceRequest(List<MaintainanceDto> GeneralConsultanCPPTDtos) : IRequest<List<MaintainanceDto>>
        {
            public List<MaintainanceDto> MaintainanceDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateMaintainanceRequest(MaintainanceDto MaintainanceDto) : IRequest<MaintainanceDto>
        {
            public MaintainanceDto MaintainanceDto { get; set; } = MaintainanceDto;
        }

        public class UpdateListMaintainanceRequest(List<MaintainanceDto> MaintainanceDtos) : IRequest<List<MaintainanceDto>>
        {
            public List<MaintainanceDto> MaintainanceDtos { get; set; } = MaintainanceDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteMaintainanceRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
