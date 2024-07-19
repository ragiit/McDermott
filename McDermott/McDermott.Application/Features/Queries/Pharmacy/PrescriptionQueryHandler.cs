using static McDermott.Application.Features.Commands.Pharmacy.PrescriptionCommand;

namespace McDermott.Application.Features.Queries.Pharmacy
{
    public class PrescriptionQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetPrescriptionQuery, List<PrescriptionDto>>,
        IRequestHandler<CreatePrescriptionRequest, PrescriptionDto>,
        IRequestHandler<CreateListPrescriptionRequest, List<PrescriptionDto>>,
        IRequestHandler<UpdatePrescriptionRequest, PrescriptionDto>,
        IRequestHandler<UpdateListPrescriptionRequest, List<PrescriptionDto>>,
        IRequestHandler<DeletePrescriptionRequest, bool>,
        IRequestHandler<GetStockOutPrescriptionQuery, List<StockOutPrescriptionDto>>,
        IRequestHandler<CreateStockOutPrescriptionRequest, StockOutPrescriptionDto>,
        IRequestHandler<CreateListStockOutPrescriptionRequest, List<StockOutPrescriptionDto>>,
        IRequestHandler<UpdateStockOutPrescriptionRequest, StockOutPrescriptionDto>,
        IRequestHandler<UpdateListStockOutPrescriptionRequest, List<StockOutPrescriptionDto>>,
        IRequestHandler<DeleteStockOutPrescriptionRequest, bool>
    {
        #region Prescription
        #region GET

        public async Task<List<PrescriptionDto>> Handle(GetPrescriptionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetPrescriptionQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Prescription>? result))
                {
                    result = await _unitOfWork.Repository<Prescription>().Entities
                       .Include(x => x.Product)
                       .Include(x => x.DrugForm)
                       .Include(x => x.Pharmacy)
                       .Include(x => x.Signa)
                       .Include(x => x.DrugRoute)
                       .Include(x => x.MedicamentGroup)
                       .Include(x => x.DrugDosage)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<PrescriptionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<PrescriptionDto> Handle(CreatePrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Prescription>().AddAsync(request.PrescriptionDto.Adapt<Prescription>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<PrescriptionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PrescriptionDto>> Handle(CreateListPrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Prescription>().AddAsync(request.PrescriptionDtos.Adapt<List<Prescription>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<PrescriptionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<PrescriptionDto> Handle(UpdatePrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Prescription>().UpdateAsync(request.PrescriptionDto.Adapt<Prescription>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<PrescriptionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PrescriptionDto>> Handle(UpdateListPrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Prescription>().UpdateAsync(request.PrescriptionDtos.Adapt<List<Prescription>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<PrescriptionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeletePrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Prescription>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Prescription>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE
        #endregion

        #region Stock Out Prescription
        #region GET

        public async Task<List<StockOutPrescriptionDto>> Handle(GetStockOutPrescriptionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetStockOutPrescriptionQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<StockOutPrescription>? result))
                {
                    result = await _unitOfWork.Repository<StockOutPrescription>().Entities
                       .AsNoTracking()
                       .Include(x => x.Prescription)
                       .Include(x => x.TransactionStock)
                       
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<StockOutPrescriptionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<StockOutPrescriptionDto> Handle(CreateStockOutPrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockOutPrescription>().AddAsync(request.StockOutPrescriptionDto.Adapt<StockOutPrescription>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<StockOutPrescriptionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StockOutPrescriptionDto>> Handle(CreateListStockOutPrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockOutPrescription>().AddAsync(request.StockOutPrescriptionDtos.Adapt<List<StockOutPrescription>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<StockOutPrescriptionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<StockOutPrescriptionDto> Handle(UpdateStockOutPrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockOutPrescription>().UpdateAsync(request.StockOutPrescriptionDto.Adapt<StockOutPrescription>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<StockOutPrescriptionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StockOutPrescriptionDto>> Handle(UpdateListStockOutPrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockOutPrescription>().UpdateAsync(request.StockOutPrescriptionDtos.Adapt<List<StockOutPrescription>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<StockOutPrescriptionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteStockOutPrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<StockOutPrescription>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<StockOutPrescription>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE
        #endregion
    }
}
