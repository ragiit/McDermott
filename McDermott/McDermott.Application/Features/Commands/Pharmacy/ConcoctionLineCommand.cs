using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Pharmacy
{
    public class ConcoctionLineCommand
    {
        #region GET

        public class GetConcoctionLineQuery(Expression<Func<ConcoctionLine, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ConcoctionLineDto>>
        {
            public Expression<Func<ConcoctionLine, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateConcoctionLineRequest(ConcoctionLineDto ConcoctionLineDto) : IRequest<ConcoctionLineDto>
        {
            public ConcoctionLineDto ConcoctionLineDto { get; set; } = ConcoctionLineDto;
        }

        public class CreateListConcoctionLineRequest(List<ConcoctionLineDto> ConcoctionLineDtos) : IRequest<List<ConcoctionLineDto>>
        {
            public List<ConcoctionLineDto> ConcoctionLineDtos { get; set; } = ConcoctionLineDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateConcoctionLineRequest(ConcoctionLineDto ConcoctionLineDto) : IRequest<ConcoctionLineDto>
        {
            public ConcoctionLineDto ConcoctionLineDto { get; set; } = ConcoctionLineDto;
        }

        public class UpdateListConcoctionLineRequest(List<ConcoctionLineDto> ConcoctionLineDtos) : IRequest<List<ConcoctionLineDto>>
        {
            public List<ConcoctionLineDto> ConcoctionLineDtos { get; set; } = ConcoctionLineDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteConcoctionLineRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}