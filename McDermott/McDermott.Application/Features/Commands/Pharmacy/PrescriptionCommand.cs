namespace McDermott.Application.Features.Commands.Pharmacy
{
    public class PrescriptionCommand
    {
        #region GET 

        public class GetPrescriptionQuery(Expression<Func<Prescription, bool>>? predicate = null, bool removeCache = false) : IRequest<List<PrescriptionDto>>
        {
            public Expression<Func<Prescription, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion

        #region CREATE

        public class CreatePrescriptionRequest(PrescriptionDto PrescriptionDto) : IRequest<PrescriptionDto>
        {
            public PrescriptionDto PrescriptionDto { get; set; } = PrescriptionDto;
        }

        public class CreateListPrescriptionRequest(List<PrescriptionDto> PrescriptionDtos) : IRequest<List<PrescriptionDto>>
        {
            public List<PrescriptionDto> PrescriptionDtos { get; set; } = PrescriptionDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdatePrescriptionRequest(PrescriptionDto PrescriptionDto) : IRequest<PrescriptionDto>
        {
            public PrescriptionDto PrescriptionDto { get; set; } = PrescriptionDto;
        }

        public class UpdateListPrescriptionRequest(List<PrescriptionDto> PrescriptionDtos) : IRequest<List<PrescriptionDto>>
        {
            public List<PrescriptionDto> PrescriptionDtos { get; set; } = PrescriptionDtos;
        }

        #endregion Update

        #region DELETE

        public class DeletePrescriptionRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
