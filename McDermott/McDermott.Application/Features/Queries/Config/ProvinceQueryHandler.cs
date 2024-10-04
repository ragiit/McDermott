using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Config.ProvinceCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class ProvinceQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetProvinceQuery, (List<ProvinceDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<ValidateProvinceQuery, bool>,
        IRequestHandler<CreateProvinceRequest, ProvinceDto>,
        IRequestHandler<BulkValidateProvinceQuery, List<ProvinceDto>>,
        IRequestHandler<CreateListProvinceRequest, List<ProvinceDto>>,
        IRequestHandler<UpdateProvinceRequest, ProvinceDto>,
        IRequestHandler<UpdateListProvinceRequest, List<ProvinceDto>>,
        IRequestHandler<DeleteProvinceRequest, bool>
    {
        #region GET

        public async Task<List<ProvinceDto>> Handle(BulkValidateProvinceQuery request, CancellationToken cancellationToken)
        {
            var ProvinceDtos = request.ProvincesToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var ProvinceNames = ProvinceDtos.Select(x => x.Name).Distinct().ToList();
            var a = ProvinceDtos.Select(x => x.CountryId).Distinct().ToList();

            var existingProvinces = await _unitOfWork.Repository<Province>()
                .Entities
                .AsNoTracking()
                .Where(v => ProvinceNames.Contains(v.Name)
                            && a.Contains(v.CountryId))
                .ToListAsync(cancellationToken);

            return existingProvinces.Adapt<List<ProvinceDto>>();
        }

        public async Task<bool> Handle(ValidateProvinceQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Province>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<ProvinceDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetProvinceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Province>().Entities.AsNoTracking();

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
                        EF.Functions.Like(v.Country.Name, $"%{request.SearchTerm}%")
                        );
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

                return (pagedItems.Adapt<List<ProvinceDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<ProvinceDto> Handle(CreateProvinceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Province>().AddAsync(request.ProvinceDto.Adapt<CreateUpdateProvinceDto>().Adapt<Province>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProvinceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ProvinceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProvinceDto>> Handle(CreateListProvinceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Province>().AddAsync(request.ProvinceDtos.Adapt<List<Province>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProvinceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ProvinceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ProvinceDto> Handle(UpdateProvinceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Province>().UpdateAsync(request.ProvinceDto.Adapt<CreateUpdateProvinceDto>().Adapt<Province>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProvinceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ProvinceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProvinceDto>> Handle(UpdateListProvinceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Province>().UpdateAsync(request.ProvinceDtos.Adapt<List<Province>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProvinceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ProvinceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteProvinceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Province>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Province>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProvinceQuery_"); // Ganti dengan key yang sesuai

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