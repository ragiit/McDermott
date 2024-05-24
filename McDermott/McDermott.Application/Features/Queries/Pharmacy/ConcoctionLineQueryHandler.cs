using static McDermott.Application.Features.Commands.Pharmacy.ConcoctionLineCommand;

namespace McDermott.Application.Features.Queries.Pharmacy
{
    public class ConcoctionLineQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetConcoctionLineQuery, List<ConcoctionLineDto>>,
        IRequestHandler<CreateConcoctionLineRequest, ConcoctionLineDto>,
        IRequestHandler<CreateListConcoctionLineRequest, List<ConcoctionLineDto>>,
        IRequestHandler<UpdateConcoctionLineRequest, ConcoctionLineDto>,
        IRequestHandler<UpdateListConcoctionLineRequest, List<ConcoctionLineDto>>,
        IRequestHandler<DeleteConcoctionLineRequest, bool>
    {
        #region GET

        public async Task<List<ConcoctionLineDto>> Handle(GetConcoctionLineQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetConcoctionLineQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<ConcoctionLine>? result))
                {
                    result = await _unitOfWork.Repository<ConcoctionLine>().Entities
                       .Include(x => x.Concoction)
                       .Include(x => x.ActiveComponent)
                       .Include(x => x.Uom)
                       .Include(x => x.MedicamentGroup)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ConcoctionLineDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<ConcoctionLineDto> Handle(CreateConcoctionLineRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ConcoctionLine>().AddAsync(request.ConcoctionLineDto.Adapt<ConcoctionLine>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionLineQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ConcoctionLineDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ConcoctionLineDto>> Handle(CreateListConcoctionLineRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ConcoctionLine>().AddAsync(request.ConcoctionLineDtos.Adapt<List<ConcoctionLine>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionLineQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ConcoctionLineDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ConcoctionLineDto> Handle(UpdateConcoctionLineRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ConcoctionLine>().UpdateAsync(request.ConcoctionLineDto.Adapt<ConcoctionLine>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionLineQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ConcoctionLineDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ConcoctionLineDto>> Handle(UpdateListConcoctionLineRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ConcoctionLine>().UpdateAsync(request.ConcoctionLineDtos.Adapt<List<ConcoctionLine>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionLineQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ConcoctionLineDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteConcoctionLineRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<ConcoctionLine>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<ConcoctionLine>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionLineQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE
    }
}