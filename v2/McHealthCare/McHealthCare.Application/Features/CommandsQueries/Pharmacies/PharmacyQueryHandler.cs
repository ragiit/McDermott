using static McHealthCare.Application.Features.Commands.Pharmacies.PharmacyCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Pharmacies
{
    public class PharmacyQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetPharmacyQuery, List<PharmacyDto>>,
        IRequestHandler<CreatePharmacyRequest, PharmacyDto>,
        IRequestHandler<CreateListPharmacyRequest, List<PharmacyDto>>,
        IRequestHandler<UpdatePharmacyRequest, PharmacyDto>,
        IRequestHandler<UpdateListPharmacyRequest, List<PharmacyDto>>,
        IRequestHandler<DeletePharmacyRequest, bool>,
        IRequestHandler<GetPharmacyLogQuery, List<PharmacyLogDto>>,
        IRequestHandler<CreatePharmacyLogRequest, PharmacyLogDto>,
        IRequestHandler<CreateListPharmacyLogRequest, List<PharmacyLogDto>>,
        IRequestHandler<UpdatePharmacyLogRequest, PharmacyLogDto>,
        IRequestHandler<UpdateListPharmacyLogRequest, List<PharmacyLogDto>>,
        IRequestHandler<DeletePharmacyLogRequest, bool>
    {
        #region Pharmacy

        #region GET

        public async Task<List<PharmacyDto>> Handle(GetPharmacyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetPharmacyQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Pharmacy>? result))
                {
                    result = await _unitOfWork.Repository<Pharmacy>().Entities
                       .AsNoTracking()
                       .Include(x => x.Location)
                       .Include(x => x.MedicamentGroup)
                       .Include(x => x.Service)
                       .Include(x => x.Practitioner)
                       .Include(x => x.Patient)
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<PharmacyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<PharmacyDto> Handle(CreatePharmacyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Pharmacy>().AddAsync(request.PharmacyDto.Adapt<Pharmacy>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<PharmacyDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PharmacyDto>> Handle(CreateListPharmacyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Pharmacy>().AddAsync(request.PharmacyDtos.Adapt<List<Pharmacy>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<PharmacyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<PharmacyDto> Handle(UpdatePharmacyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Pharmacy>().UpdateAsync(request.PharmacyDto.Adapt<Pharmacy>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<PharmacyDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PharmacyDto>> Handle(UpdateListPharmacyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Pharmacy>().UpdateAsync(request.PharmacyDtos.Adapt<List<Pharmacy>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<PharmacyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeletePharmacyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty)
                {
                    await _unitOfWork.Repository<Pharmacy>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Pharmacy>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE

        #endregion Pharmacy

        #region Pharmacy Log

        #region GET

        public async Task<List<PharmacyLogDto>> Handle(GetPharmacyLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetPharmacyLogQuery";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<PharmacyLog>? result))
                {
                    result = await _unitOfWork.Repository<PharmacyLog>().Entities
                        .Include(x => x.Pharmacy)
                        .Include(x => x.UserBy)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }
                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<PharmacyLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<PharmacyLogDto> Handle(CreatePharmacyLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<PharmacyLog>().AddAsync(request.PharmacyLogDto.Adapt<PharmacyLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<PharmacyLogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PharmacyLogDto>> Handle(CreateListPharmacyLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<PharmacyLog>().AddAsync(request.PharmacyLogDtos.Adapt<List<PharmacyLog>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<PharmacyLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<PharmacyLogDto> Handle(UpdatePharmacyLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<PharmacyLog>().UpdateAsync(request.PharmacyLogDto.Adapt<PharmacyLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<PharmacyLogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PharmacyLogDto>> Handle(UpdateListPharmacyLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<PharmacyLog>().UpdateAsync(request.PharmacyLogDtos.Adapt<List<PharmacyLog>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<PharmacyLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeletePharmacyLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty)
                {
                    await _unitOfWork.Repository<PharmacyLog>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Pharmacy>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyLogQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE

        #endregion Pharmacy Log
    }
}