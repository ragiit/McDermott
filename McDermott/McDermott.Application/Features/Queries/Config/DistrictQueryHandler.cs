using Microsoft.EntityFrameworkCore.Internal;
using static McDermott.Application.Features.Commands.Config.DistrictCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class DistrictQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDistrictQuery, (List<DistrictDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<ValidateDistrictQuery, bool>,
        IRequestHandler<CreateDistrictRequest, DistrictDto>,
        IRequestHandler<CreateListDistrictRequest, List<DistrictDto>>,
        IRequestHandler<UpdateDistrictRequest, DistrictDto>,
        IRequestHandler<UpdateListDistrictRequest, List<DistrictDto>>,
        IRequestHandler<DeleteDistrictRequest, bool>
    {
        #region GET

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

                var skip = (request.PageIndex) * request.PageSize;

                var totalCount = await query.CountAsync(cancellationToken);

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

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