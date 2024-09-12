using static McDermott.Application.Features.Commands.Config.CountryCommand;
using static McDermott.Application.Features.Commands.Config.VillageCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class CountryQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetCountryQuery, (List<CountryDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<ValidateCountryQuery, bool>,
        IRequestHandler<CreateCountryRequest, CountryDto>,
        IRequestHandler<CreateListCountryRequest, List<CountryDto>>,
        IRequestHandler<UpdateCountryRequest, CountryDto>,
        IRequestHandler<UpdateListCountryRequest, List<CountryDto>>,
        IRequestHandler<DeleteCountryRequest, bool>
    {
        #region CREATE

        public async Task<CountryDto> Handle(CreateCountryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Country>().AddAsync(request.CountryDto.Adapt<Country>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCountryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<CountryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CountryDto>> Handle(CreateListCountryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Country>().AddAsync(request.CountryDtos.Adapt<List<Country>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCountryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<CountryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<CountryDto> Handle(UpdateCountryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Country>().UpdateAsync(request.CountryDto.Adapt<Country>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCountryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<CountryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CountryDto>> Handle(UpdateListCountryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Country>().UpdateAsync(request.CountryDtos.Adapt<List<Country>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCountryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<CountryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteCountryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Country>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Country>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCountryQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(List<CountryDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetCountryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Country>().Entities
                    .AsNoTracking()
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%"));
                }

                var pagedResult = query
                            .OrderBy(x => x.Name);

                var skip = (request.PageIndex) * request.PageSize;

                var totalCount = await query.CountAsync(cancellationToken);

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<CountryDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateCountryQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Country>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion DELETE
    }
}