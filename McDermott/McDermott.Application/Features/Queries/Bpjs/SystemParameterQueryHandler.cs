namespace McDermott.Application.Features.Queries.Bpjs
{
    public class SystemParameterQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetSystemParameterQuery, List<SystemParameterDto>>,
        IRequestHandler<CreateSystemParameterRequest, SystemParameterDto>,
        IRequestHandler<CreateListSystemParameterRequest, List<SystemParameterDto>>,
        IRequestHandler<UpdateSystemParameterRequest, SystemParameterDto>,
        IRequestHandler<UpdateListSystemParameterRequest, List<SystemParameterDto>>,
        IRequestHandler<DeleteSystemParameterRequest, bool>
    {
        #region GET

        public async Task<List<SystemParameterDto>> Handle(GetSystemParameterQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetSystemParameterQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<SystemParameter>? result))
                {
                    result = await _unitOfWork.Repository<SystemParameter>().Entities
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    //result = await _unitOfWork.Repository<SystemParameter>().GetAsync(
                    //    null,
                    //    x => x.Include(z => z.Country),
                    //    cancellationToken);

                    //return await _unitOfWork.Repository<Counter>().Entities
                    //  .Include(x => x.Physician)
                    //  .Include(x => x.Service)
                    //  .AsNoTracking()
                    //  .Select(Counter => Counter.Adapt<CounterDto>())
                    //  .AsNoTracking()
                    //  .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<SystemParameterDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<SystemParameterDto> Handle(CreateSystemParameterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<SystemParameter>().AddAsync(request.SystemParameterDto.Adapt<SystemParameter>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSystemParameterQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<SystemParameterDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SystemParameterDto>> Handle(CreateListSystemParameterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<SystemParameter>().AddAsync(request.SystemParameterDtos.Adapt<List<SystemParameter>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSystemParameterQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<SystemParameterDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<SystemParameterDto> Handle(UpdateSystemParameterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<SystemParameter>().UpdateAsync(request.SystemParameterDto.Adapt<SystemParameter>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSystemParameterQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<SystemParameterDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SystemParameterDto>> Handle(UpdateListSystemParameterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<SystemParameter>().UpdateAsync(request.SystemParameterDtos.Adapt<List<SystemParameter>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSystemParameterQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<SystemParameterDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteSystemParameterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<SystemParameter>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<SystemParameter>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSystemParameterQuery_"); // Ganti dengan key yang sesuai

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