using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Pharmacy
{
    public class MedicamentCommand
    {
        #region GET 

        public class GetMedicamentQuery(Expression<Func<Medicament, bool>>? predicate = null, bool removeCache = false) : IRequest<List<MedicamentDto>>
        {
            public Expression<Func<Medicament, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateMedicamentRequest(MedicamentDto MedicamentDto) : IRequest<MedicamentDto>
        {
            public MedicamentDto MedicamentDto { get; set; } = MedicamentDto;
        }

        public class CreateListMedicamentRequest(List<MedicamentDto> MedicamentDtos) : IRequest<List<MedicamentDto>>
        {
            public List<MedicamentDto> MedicamentDtos { get; set; } = MedicamentDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateMedicamentRequest(MedicamentDto MedicamentDto) : IRequest<MedicamentDto>
        {
            public MedicamentDto MedicamentDto { get; set; } = MedicamentDto;
        }

        public class UpdateListMedicamentRequest(List<MedicamentDto> MedicamentDtos) : IRequest<List<MedicamentDto>>
        {
            public List<MedicamentDto> MedicamentDtos { get; set; } = MedicamentDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteMedicamentRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
