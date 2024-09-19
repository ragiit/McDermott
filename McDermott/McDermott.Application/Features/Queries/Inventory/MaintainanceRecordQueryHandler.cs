using McDermott.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Inventory.MaintinanceRecordCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class MaintainanceRecordQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetAllMaintainanceRecordQuery, List<MaintainanceRecordDto>>,
        IRequestHandler<GetMaintainanceRecordQuery, (List<MaintainanceRecordDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateMaintainanceRecordRequest, MaintainanceRecordDto>,
        IRequestHandler<CreateListMaintainanceRecordRequest, List<MaintainanceRecordDto>>,
        IRequestHandler<UpdateMaintainanceRecordRequest, MaintainanceRecordDto>,
        IRequestHandler<UpdateListMaintainanceRecordRequest, List<MaintainanceRecordDto>>,
        IRequestHandler<DeleteMaintainanceRecordRequest, bool>
    {
        #region GET

        public async Task<List<MaintainanceRecordDto>> Handle(GetAllMaintainanceRecordQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllMaintainanceRecordQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<MaintainanceRecord>? result))
                {
                    result = await _unitOfWork.Repository<MaintainanceRecord>().GetAsync(
                        null,
                        x => x
                        .Include(z => z.Maintainance)
                        .Include(x => x.Product),
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<MaintainanceRecordDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(List<MaintainanceRecordDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetMaintainanceRecordQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<MaintainanceRecord>().Entities
                    .Include(x => x.Product)
                    .Include(x => x.Maintainance)
                    .AsNoTracking()
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.DocumentName, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.SequenceProduct, $"%{request.SearchTerm}%"));
                }

                var totalCount = await query.CountAsync(cancellationToken);
                var pagedResult = query
                            .OrderBy(x => x.SequenceProduct);

                var skip = (request.PageIndex) * request.PageSize;

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<MaintainanceRecordDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateMaintainanceRecordQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<MaintainanceRecord>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<MaintainanceRecordDto> Handle(CreateMaintainanceRecordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaintainanceRecord>().AddAsync(request.MaintainanceRecordDto.Adapt<MaintainanceRecord>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintainanceRecordQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MaintainanceRecordDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MaintainanceRecordDto>> Handle(CreateListMaintainanceRecordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaintainanceRecord>().AddAsync(request.MaintainanceRecordDtos.Adapt<List<MaintainanceRecord>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintainanceRecordQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MaintainanceRecordDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<MaintainanceRecordDto> Handle(UpdateMaintainanceRecordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaintainanceRecord>().UpdateAsync(request.MaintainanceRecordDto.Adapt<MaintainanceRecord>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintainanceRecordQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MaintainanceRecordDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MaintainanceRecordDto>> Handle(UpdateListMaintainanceRecordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaintainanceRecord>().UpdateAsync(request.MaintainanceRecordDtos.Adapt<List<MaintainanceRecord>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintainanceRecordQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MaintainanceRecordDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteMaintainanceRecordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<MaintainanceRecord>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<MaintainanceRecord>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintainanceRecordQuery_"); // Ganti dengan key yang sesuai

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
