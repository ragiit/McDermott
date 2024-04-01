using McDermott.Application.Dtos.Pharmacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Pharmacy
{
    public class SignaCommand
    {

        #region GET

        public class GetSignaQuery(Expression<Func<Signa, bool>>? predicate = null, bool removeCache = false) : IRequest<List<SignaDto>>
        {
            public Expression<Func<Signa, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateSignaRequest(SignaDto SignaDto) : IRequest<SignaDto>
        {
            public SignaDto SignaDto { get; set; } = SignaDto;
        }
        public class CreateListSignaRequest(List<SignaDto> Signas) : IRequest<List<SignaDto>>
        {
            public List<SignaDto> SignaDtos { get; set; } = Signas;
        }

        #endregion CREATE

        #region Update

        public class UpdateSignaRequest(SignaDto SignaDto) : IRequest<SignaDto>
        {
            public SignaDto SignaDto { get; set; } = SignaDto;
        }

        public class UpdateListSignaRequest(List<SignaDto> SignaDtos) : IRequest<List<SignaDto>>
        {
            public List<SignaDto> SignaDtos { get; set; } = SignaDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteSignaRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
