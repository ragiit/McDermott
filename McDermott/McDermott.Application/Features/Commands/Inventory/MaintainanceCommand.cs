using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Inventory
{
    public class MaintainanceCommand
    {
        #region GET 

        public class GetMaintainanceQuery(Expression<Func<Maintainance, bool>>? predicate = null, bool removeCache = false) : IRequest<List<MaintainanceDto>>
        {
            public Expression<Func<Maintainance, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateMaintainanceRequest(MaintainanceDto MaintainanceDto) : IRequest<MaintainanceDto>
        {
            public MaintainanceDto MaintainanceDto { get; set; } = MaintainanceDto;
        }

        public class CreateListMaintainanceRequest(List<MaintainanceDto> MaintainanceDtos) : IRequest<List<MaintainanceDto>>
        {
            public List<MaintainanceDto> MaintainanceDtos { get; set; } = MaintainanceDtos;
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
