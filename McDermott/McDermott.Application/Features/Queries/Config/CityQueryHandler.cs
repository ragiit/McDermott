using static McDermott.Application.Features.Commands.Config.CityCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class CityQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetCityQuery, (List<CityDto>, int pageIndex, int pageSize, int pageCount)>,
  IRequestHandler<ValidateCityQuery, bool>,
        IRequestHandler<CreateCityRequest, CityDto>,
        IRequestHandler<CreateListCityRequest, List<CityDto>>,
        IRequestHandler<UpdateCityRequest, CityDto>,
        IRequestHandler<UpdateListCityRequest, List<CityDto>>,
        IRequestHandler<DeleteCityRequest, bool>
    {
        #region GET

        public async Task<(List<CityDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetCityQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<City>().Entities
                    .AsNoTracking()
                    .Include(v => v.Province)
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Province.Name, $"%{request.SearchTerm}%"));
                }

                var pagedResult = query
                            .OrderBy(x => x.Name);

                var totalCount = await query.CountAsync(cancellationToken);

                var skip = (request.PageIndex) * (request.PageSize == 0 ? totalCount : request.PageSize);

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<CityDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateCityQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<City>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<CityDto> Handle(CreateCityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<City>().AddAsync(request.CityDto.Adapt<City>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCityQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<CityDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CityDto>> Handle(CreateListCityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<City>().AddAsync(request.CityDtos.Adapt<List<City>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCityQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<CityDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<CityDto> Handle(UpdateCityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<City>().UpdateAsync(request.CityDto.Adapt<City>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCityQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<CityDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CityDto>> Handle(UpdateListCityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<City>().UpdateAsync(request.CityDtos.Adapt<List<City>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCityQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<CityDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteCityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<City>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<City>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCityQuery_"); // Ganti dengan key yang sesuai

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