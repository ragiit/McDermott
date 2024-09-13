using McDermott.Application.Features.Services;
using Microsoft.EntityFrameworkCore.Internal;
using static McDermott.Application.Features.Commands.Config.DistrictCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class DistrictQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDistrictQuery, (List<DistrictDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<ValidateDistrictQuery, bool>,
        IRequestHandler<BulkValidateDistrictQuery, List<DistrictDto>>,
        IRequestHandler<CreateDistrictRequest, DistrictDto>,
        IRequestHandler<CreateListDistrictRequest, List<DistrictDto>>,
        IRequestHandler<UpdateDistrictRequest, DistrictDto>,
        IRequestHandler<UpdateListDistrictRequest, List<DistrictDto>>,
        IRequestHandler<DeleteDistrictRequest, bool>
    {
        #region GET

        public async Task<List<DistrictDto>> Handle(BulkValidateDistrictQuery request, CancellationToken cancellationToken)
        {
            var DistrictDtos = request.DistrictsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var DistrictNames = DistrictDtos.Select(x => x.Name).Distinct().ToList();
            var provinceIds = DistrictDtos.Select(x => x.ProvinceId).Distinct().ToList();
            var cityIds = DistrictDtos.Select(x => x.CityId).Distinct().ToList();

            var existingDistricts = await _unitOfWork.Repository<District>()
                .Entities
                .AsNoTracking()
                .Where(v => DistrictNames.Contains(v.Name)
                            && provinceIds.Contains(v.ProvinceId)
                            && cityIds.Contains(v.CityId))
                .ToListAsync(cancellationToken);

            return existingDistricts.Adapt<List<DistrictDto>>();
        }

        public async Task<(List<DistrictDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetDistrictQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<District>().Entities
                    .AsNoTracking()
                    .Include(v => v.City)
                    .Include(v => v.Province)
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.City.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Province.Name, $"%{request.SearchTerm}%"));
                }

                var pagedResult = query
                            .OrderBy(x => x.Name);

                var (totalCount, paged, totalPages) = await PaginateAsyncClass.PaginateAsync(request.PageSize, request.PageIndex, query, pagedResult, cancellationToken);

                return (paged.Adapt<List<DistrictDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateDistrictQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<District>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<DistrictDto> Handle(CreateDistrictRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<District>().AddAsync(request.DistrictDto.Adapt<District>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDistrictQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DistrictDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DistrictDto>> Handle(CreateListDistrictRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<District>().AddAsync(request.DistrictDtos.Adapt<List<District>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDistrictQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DistrictDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DistrictDto> Handle(UpdateDistrictRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<District>().UpdateAsync(request.DistrictDto.Adapt<District>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDistrictQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DistrictDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DistrictDto>> Handle(UpdateListDistrictRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<District>().UpdateAsync(request.DistrictDtos.Adapt<List<District>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDistrictQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DistrictDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDistrictRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<District>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<District>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDistrictQuery_"); // Ganti dengan key yang sesuai

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