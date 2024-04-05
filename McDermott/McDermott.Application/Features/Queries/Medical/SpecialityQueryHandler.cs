namespace McDermott.Application.Features.Queries.Medical
{
    public class SpecialityQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetSpecialityQuery, List<SpecialityDto>>,
        IRequestHandler<CreateSpecialityRequest, SpecialityDto>,
        IRequestHandler<CreateListSpecialityRequest, List<SpecialityDto>>,
        IRequestHandler<UpdateSpecialityRequest, SpecialityDto>,
        IRequestHandler<UpdateListSpecialityRequest, List<SpecialityDto>>,
        IRequestHandler<DeleteSpecialityRequest, bool>
    {
        #region GET

        public async Task<List<SpecialityDto>> Handle(GetSpecialityQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetSpecialityQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Speciality>? result))
                {
                    result = await _unitOfWork.Repository<Speciality>().Entities
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<SpecialityDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<SpecialityDto> Handle(CreateSpecialityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Speciality>().AddAsync(request.SpecialityDto.Adapt<Speciality>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSpecialityQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<SpecialityDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SpecialityDto>> Handle(CreateListSpecialityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Speciality>().AddAsync(request.SpecialityDtos.Adapt<List<Speciality>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSpecialityQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<SpecialityDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<SpecialityDto> Handle(UpdateSpecialityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Speciality>().UpdateAsync(request.SpecialityDto.Adapt<Speciality>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSpecialityQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<SpecialityDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SpecialityDto>> Handle(UpdateListSpecialityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Speciality>().UpdateAsync(request.SpecialityDtos.Adapt<List<Speciality>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSpecialityQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<SpecialityDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteSpecialityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Speciality>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Speciality>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSpecialityQuery_"); // Ganti dengan key yang sesuai

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