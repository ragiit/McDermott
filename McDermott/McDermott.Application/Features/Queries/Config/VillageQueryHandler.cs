using static McDermott.Application.Features.Commands.Config.VillageCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class VillageQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetVillageQuery, List<VillageDto>>,
        IRequestHandler<CreateVillageRequest, VillageDto>,
        IRequestHandler<CreateListVillageRequest, List<VillageDto>>,
        IRequestHandler<UpdateVillageRequest, VillageDto>,
        IRequestHandler<UpdateListVillageRequest, List<VillageDto>>,
        IRequestHandler<DeleteVillageRequest, bool>
    {
        #region GET

        public async Task<List<VillageDto>> Handle(GetVillageQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetVillageQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Village>? result))
                {
                    //return await _unitOfWork.Repository<DetailQueueDisplay>().Entities
                    //    .Include(x => x.QueueDisplay)
                    //    .Include(x => x.Counter)
                    //    .Select(DetailQueueDisplay => DetailQueueDisplay.Adapt<DetailQueueDisplayDto>())
                    //    .AsNoTracking()
                    //    .ToListAsync(cancellationToken);

                    result = await _unitOfWork.Repository<Village>().GetAsync(
                        null,
                        x => x
                        .Include(z => z.Province)
                        .Include(z => z.City)
                        .Include(z => z.District),
                        cancellationToken);

                    //result = await _unitOfWork.Repository<Village>().Entities
                    //    .Include(x => x.Province)
                    //    .Include(x => x.City)
                    //    .Include(x => x.District)
                    //    .OrderBy(x => x.Name)
                    //    .Take(5)
                    //    .AsNoTracking()
                    //    .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<VillageDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion GET

        #region CREATE

        public async Task<VillageDto> Handle(CreateVillageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Village>().AddAsync(request.VillageDto.Adapt<Village>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetVillageQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<VillageDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<VillageDto>> Handle(CreateListVillageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Village>().AddAsync(request.VillageDtos.Adapt<List<Village>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetVillageQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<VillageDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<VillageDto> Handle(UpdateVillageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Village>().UpdateAsync(request.VillageDto.Adapt<Village>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetVillageQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<VillageDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<VillageDto>> Handle(UpdateListVillageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Village>().UpdateAsync(request.VillageDtos.Adapt<List<Village>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetVillageQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<VillageDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteVillageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Village>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Village>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetVillageQuery_"); // Ganti dengan key yang sesuai

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