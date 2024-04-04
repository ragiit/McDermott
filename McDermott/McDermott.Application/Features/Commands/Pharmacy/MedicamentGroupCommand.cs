using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Pharmacy
{
    public class MedicamentGroupCommand
    {
        #region GET 

        public class GetMedicamentGroupQuery(Expression<Func<MedicamentGroup, bool>>? predicate = null, bool removeCache = false) : IRequest<List<MedicamentGroupDto>>
        {
            public Expression<Func<MedicamentGroup, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateMedicamentGroupRequest(MedicamentGroupDto MedicamentGroupDto) : IRequest<MedicamentGroupDto>
        {
            public MedicamentGroupDto MedicamentGroupDto { get; set; } = MedicamentGroupDto;
        }

        public class CreateListMedicamentGroupRequest(List<MedicamentGroupDto> MedicamentGroupDtos) : IRequest<List<MedicamentGroupDto>>
        {
            public List<MedicamentGroupDto> MedicamentGroupDtos { get; set; } = MedicamentGroupDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateMedicamentGroupRequest(MedicamentGroupDto MedicamentGroupDto) : IRequest<MedicamentGroupDto>
        {
            public MedicamentGroupDto MedicamentGroupDto { get; set; } = MedicamentGroupDto;
        }

        public class UpdateListMedicamentGroupRequest(List<MedicamentGroupDto> MedicamentGroupDtos) : IRequest<List<MedicamentGroupDto>>
        {
            public List<MedicamentGroupDto> MedicamentGroupDtos { get; set; } = MedicamentGroupDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteMedicamentGroupRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
