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
        public class GetSingleMedicamentQuery : IRequest<MedicamentDto>
        {
            public List<Expression<Func<Medicament, object>>> Includes { get; set; }
            public Expression<Func<Medicament, bool>> Predicate { get; set; }
            public Expression<Func<Medicament, Medicament>> Select { get; set; }
            public Expression<Func<Medicament, object>> OrderBy { get; set; }
            public bool IsDescending { get; set; } = false; // default to ascending
        }
        public class GetMedicamentQuery : IRequest<(List<MedicamentDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Medicament, object>>> Includes { get; set; }
            public Expression<Func<Medicament, bool>> Predicate { get; set; }
            public Expression<Func<Medicament, Medicament>> Select { get; set; }
            public Expression<Func<Medicament, object>> OrderBy { get; set; }
            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateMedicamentQuery(List<MedicamentDto> MedicamentsToValidate) : IRequest<List<MedicamentDto>>
        {
            public List<MedicamentDto> MedicamentsToValidate { get; } = MedicamentsToValidate;
        }

        public class ValidateMedicamentQuery(Expression<Func<Medicament, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Medicament, bool>> Predicate { get; } = predicate!;
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
