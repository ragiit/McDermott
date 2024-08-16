namespace McHealthCare.Application.Features.Commands.Pharmacies
{
    public class PrescriptionCommand
    {
        #region Prescription

        #region GET

        public class GetPrescriptionQuery(Expression<Func<Prescription, bool>>? predicate = null, bool removeCache = false) : IRequest<List<PrescriptionDto>>
        {
            public Expression<Func<Prescription, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

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

        public class DeletePrescriptionRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE

        #endregion Prescription

        #region Cut Stock Prescription

        #region GET

        public class GetStockOutPrescriptionQuery(Expression<Func<StockOutPrescription, bool>>? predicate = null, bool removeCache = false) : IRequest<List<StockOutPrescriptionDto>>
        {
            public Expression<Func<StockOutPrescription, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET



        #region CREATE

        public class CreateStockOutPrescriptionRequest(StockOutPrescriptionDto StockOutPrescriptionDto) : IRequest<StockOutPrescriptionDto>
        {
            public StockOutPrescriptionDto StockOutPrescriptionDto { get; set; } = StockOutPrescriptionDto;
        }

        public class CreateListStockOutPrescriptionRequest(List<StockOutPrescriptionDto> StockOutPrescriptionDtos) : IRequest<List<StockOutPrescriptionDto>>
        {
            public List<StockOutPrescriptionDto> StockOutPrescriptionDtos { get; set; } = StockOutPrescriptionDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateStockOutPrescriptionRequest(StockOutPrescriptionDto StockOutPrescriptionDto) : IRequest<StockOutPrescriptionDto>
        {
            public StockOutPrescriptionDto StockOutPrescriptionDto { get; set; } = StockOutPrescriptionDto;
        }

        public class UpdateListStockOutPrescriptionRequest(List<StockOutPrescriptionDto> StockOutPrescriptionDtos) : IRequest<List<StockOutPrescriptionDto>>
        {
            public List<StockOutPrescriptionDto> StockOutPrescriptionDtos { get; set; } = StockOutPrescriptionDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteStockOutPrescriptionRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE

        #endregion Cut Stock Prescription
    }
}