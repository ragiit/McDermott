using static McHealthCare.Application.Features.Commands.Pharmacies.ConcoctionCommand;
using static McHealthCare.Application.Features.Commands.Pharmacies.ConcoctionLineCommand;

namespace McHealthCare.Application.Features.Queries.Pharmacy
{
    public class ConcoctionQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetConcoctionQuery, List<ConcoctionDto>>,
        IRequestHandler<CreateConcoctionRequest, ConcoctionDto>,
        IRequestHandler<CreateListConcoctionRequest, List<ConcoctionDto>>,
        IRequestHandler<UpdateConcoctionRequest, ConcoctionDto>,
        IRequestHandler<UpdateListConcoctionRequest, List<ConcoctionDto>>,
        IRequestHandler<DeleteConcoctionRequest, bool>,
        IRequestHandler<GetStockOutLineQuery, List<StockOutLinesDto>>,
        IRequestHandler<CreateStockOutLinesRequest, StockOutLinesDto>,
        IRequestHandler<CreateListStockOutLinesRequest, List<StockOutLinesDto>>,
        IRequestHandler<UpdateStockOutLinesRequest, StockOutLinesDto>,
        IRequestHandler<UpdateListStockOutLinesRequest, List<StockOutLinesDto>>,
        IRequestHandler<DeleteStockOutLinesRequest, bool>
    {
        #region ConcoctionLine

        #region GET

        public async Task<List<ConcoctionDto>> Handle(GetConcoctionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetConcoctionQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Concoction>? result))
                {
                    result = await _unitOfWork.Repository<Concoction>().Entities
                       .AsNoTracking()
                       .Include(x => x.Pharmacy)
                       .Include(x => x.MedicamentGroup)
                       .Include(x => x.Practitioner)
                       .Include(x => x.DrugForm)
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ConcoctionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<ConcoctionDto> Handle(CreateConcoctionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Concoction>().AddAsync(request.ConcoctionDto.Adapt<Concoction>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ConcoctionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ConcoctionDto>> Handle(CreateListConcoctionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Concoction>().AddAsync(request.ConcoctionDtos.Adapt<List<Concoction>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ConcoctionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ConcoctionDto> Handle(UpdateConcoctionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Concoction>().UpdateAsync(request.ConcoctionDto.Adapt<Concoction>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ConcoctionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ConcoctionDto>> Handle(UpdateListConcoctionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Concoction>().UpdateAsync(request.ConcoctionDtos.Adapt<List<Concoction>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ConcoctionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteConcoctionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty)
                {
                    await _unitOfWork.Repository<Concoction>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Concoction>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE

        #endregion ConcoctionLine

        #region Stock Out ConcoctionLine

        #region GET

        public async Task<List<StockOutLinesDto>> Handle(GetStockOutLineQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetStockOutLinesQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<StockOutLines>? result))
                {
                    result = await _unitOfWork.Repository<StockOutLines>().Entities
                       .AsNoTracking()
                       .Include(x => x.Lines)
                       .Include(x => x.TransactionStock)

                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<StockOutLinesDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<StockOutLinesDto> Handle(CreateStockOutLinesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockOutLines>().AddAsync(request.StockOutLinesDto.Adapt<StockOutLines>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutLinesQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<StockOutLinesDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StockOutLinesDto>> Handle(CreateListStockOutLinesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockOutLines>().AddAsync(request.StockOutLinesDtos.Adapt<List<StockOutLines>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutLinesQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<StockOutLinesDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<StockOutLinesDto> Handle(UpdateStockOutLinesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockOutLines>().UpdateAsync(request.StockOutLinesDto.Adapt<StockOutLines>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutLinesQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<StockOutLinesDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StockOutLinesDto>> Handle(UpdateListStockOutLinesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockOutLines>().UpdateAsync(request.StockOutLinesDto.Adapt<List<StockOutLines>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutLinesQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<StockOutLinesDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteStockOutLinesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty)
                {
                    await _unitOfWork.Repository<StockOutLines>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<StockOutLines>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutLinesQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE

        #endregion Stock Out ConcoctionLine
    }
}