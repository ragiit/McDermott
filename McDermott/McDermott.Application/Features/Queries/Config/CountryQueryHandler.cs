using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Config.CountryCommand;
using static McDermott.Application.Features.Commands.Config.VillageCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class CountryQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetCountryQuery, (List<CountryDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<ValidateCountryQuery, bool>,
        IRequestHandler<CreateCountryRequest, CountryDto>,
        IRequestHandler<BulkValidateCountryQuery, List<CountryDto>>,
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

        #endregion DELETE

        public async Task<(List<CountryDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetCountryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Country>().Entities.AsNoTracking();

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                {
                    query = query.Select(request.Select);
                }

                var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                                  query,
                                  request.PageSize,
                                  request.PageIndex,
                                  q => q.OrderBy(x => x.Name), // Custom order by bisa diterapkan di sini
                                  cancellationToken);

                return (pagedItems.Adapt<List<CountryDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CountryDto>> Handle(BulkValidateCountryQuery request, CancellationToken cancellationToken)
        {
            var CountryDtos = request.CountrysToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var CountryNames = CountryDtos.Select(x => x.Name).Distinct().ToList();
            var Codes = CountryDtos.Select(x => x.Code).Distinct().ToList();

            var existingCountrys = await _unitOfWork.Repository<Country>()
                .Entities
                .AsNoTracking()
                .Where(v => CountryNames.Contains(v.Name) && Codes.Contains(v.Code))
                .ToListAsync(cancellationToken);

            return existingCountrys.Adapt<List<CountryDto>>();
        }

        public async Task<bool> Handle(ValidateCountryQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Country>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }
    }
}