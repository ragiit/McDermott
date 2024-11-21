namespace McDermott.Application.Features.Commands.Pharmacies
{
    public class PrescriptionCommand
    {
        #region Prescription

        #region GET Prescription

        public class GetAllPrescriptionQuery(Expression<Func<Prescription, bool>>? predicate = null, bool removeCache = false) : IRequest<List<PrescriptionDto>>
        {
            public Expression<Func<Prescription, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetSinglePrescriptionQuery : IRequest<PrescriptionDto>
        {
            public List<Expression<Func<Prescription, object>>> Includes { get; set; }
            public Expression<Func<Prescription, bool>> Predicate { get; set; }
            public Expression<Func<Prescription, Prescription>> Select { get; set; }

            public List<(Expression<Func<Prescription, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetPrescriptionQuery : IRequest<(List<PrescriptionDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Prescription, object>>> Includes { get; set; }
            public Expression<Func<Prescription, bool>> Predicate { get; set; }
            public Expression<Func<Prescription, Prescription>> Select { get; set; }

            public List<(Expression<Func<Prescription, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidatePrescriptionQuery(List<PrescriptionDto> PrescriptionToValidate) : IRequest<List<PrescriptionDto>>
        {
            public List<PrescriptionDto> PrescriptionToValidate { get; } = PrescriptionToValidate;
        }

        public class ValidatePrescriptionQuery(Expression<Func<Prescription, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Prescription, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET Prescription

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

        #endregion Prescription

        #region Cut Stock Prescription

        #region GET StockOutPrescription

        public class GetAllStockOutPrescriptionQuery(Expression<Func<StockOutPrescription, bool>>? predicate = null, bool removeCache = false) : IRequest<List<StockOutPrescriptionDto>>
        {
            public Expression<Func<StockOutPrescription, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetSingleStockOutPrescriptionQuery : IRequest<StockOutPrescriptionDto>
        {
            public List<Expression<Func<StockOutPrescription, object>>> Includes { get; set; }
            public Expression<Func<StockOutPrescription, bool>> Predicate { get; set; }
            public Expression<Func<StockOutPrescription, StockOutPrescription>> Select { get; set; }

            public List<(Expression<Func<StockOutPrescription, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetStockOutPrescriptionQuery : IRequest<(List<StockOutPrescriptionDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<StockOutPrescription, object>>> Includes { get; set; }
            public Expression<Func<StockOutPrescription, bool>> Predicate { get; set; }
            public Expression<Func<StockOutPrescription, StockOutPrescription>> Select { get; set; }

            public List<(Expression<Func<StockOutPrescription, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateStockOutPrescriptionQuery(List<StockOutPrescriptionDto> StockOutPrescriptionToValidate) : IRequest<List<StockOutPrescriptionDto>>
        {
            public List<StockOutPrescriptionDto> StockOutPrescriptionToValidate { get; } = StockOutPrescriptionToValidate;
        }

        public class ValidateStockOutPrescriptionQuery(Expression<Func<StockOutPrescription, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<StockOutPrescription, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET StockOutPrescription

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

        public class DeleteStockOutPrescriptionRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE

        #endregion Cut Stock Prescription
    }
}