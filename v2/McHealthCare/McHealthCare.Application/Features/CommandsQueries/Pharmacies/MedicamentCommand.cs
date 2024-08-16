namespace McHealthCare.Application.Features.Commands.Pharmacies
{
    public class MedicamentCommand
    {
        #region GET

        public class GetMedicamentQuery(Expression<Func<Medicament, bool>>? predicate = null, bool removeCache = false) : IRequest<List<MedicamentDto>>
        {
            public Expression<Func<Medicament, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

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

        public class DeleteMedicamentRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}