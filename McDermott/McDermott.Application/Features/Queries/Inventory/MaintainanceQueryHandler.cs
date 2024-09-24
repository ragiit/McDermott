using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Inventory.MaintainanceCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class MaintainanceQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetMaintainanceQuery, (List<MaintainanceDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetAllMaintainanceQuery, List<MaintainanceDto>>,
        IRequestHandler<ValidateMaintainanceQuery, bool>,
        IRequestHandler<CreateMaintainanceRequest, MaintainanceDto>,
        IRequestHandler<CreateListMaintainanceRequest, List<MaintainanceDto>>,
        IRequestHandler<UpdateMaintainanceRequest, MaintainanceDto>,
        IRequestHandler<UpdateListMaintainanceRequest, List<MaintainanceDto>>,
        IRequestHandler<DeleteMaintainanceRequest, bool>
    {
        #region GET

        public async Task<List<MaintainanceDto>> Handle(GetAllMaintainanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetMaintainanceQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Maintainance>? result))
                {
                    result = await _unitOfWork.Repository<Maintainance>().Entities
                     .Include(x => x.RequestBy)
                    .Include(x => x.Location)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<MaintainanceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(List<MaintainanceDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetMaintainanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Maintainance>().Entities
                    .Include(x => x.RequestBy)
                    .Include(x => x.Location)
                    .AsNoTracking()
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Title, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Sequence, $"%{request.SearchTerm}%"));
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var pagedResult = query
                            .OrderBy(x => x.Title);

                var skip = (request.PageIndex) * request.PageSize;

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<MaintainanceDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateMaintainanceQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Maintainance>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<MaintainanceDto> Handle(CreateMaintainanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Maintainance>().AddAsync(request.MaintainanceDto.Adapt<Maintainance>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintainanceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MaintainanceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MaintainanceDto>> Handle(CreateListMaintainanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Maintainance>().AddAsync(request.MaintainanceDtos.Adapt<List<Maintainance>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintainanceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MaintainanceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<MaintainanceDto> Handle(UpdateMaintainanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Maintainance>().UpdateAsync(request.MaintainanceDto.Adapt<Maintainance>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintainanceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MaintainanceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MaintainanceDto>> Handle(UpdateListMaintainanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Maintainance>().UpdateAsync(request.MaintainanceDtos.Adapt<List<Maintainance>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintainanceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MaintainanceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteMaintainanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Maintainance>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Maintainance>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintainanceQuery_"); // Ganti dengan key yang sesuai

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
