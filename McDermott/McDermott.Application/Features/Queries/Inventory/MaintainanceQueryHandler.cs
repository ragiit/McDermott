using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Inventory.MaintainanceCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class MaintainanceQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetMaintainanceQuery, List<MaintainanceDto>>,
        IRequestHandler<CreateMaintainanceRequest, MaintainanceDto>,
        IRequestHandler<CreateListMaintainanceRequest, List<MaintainanceDto>>,
        IRequestHandler<UpdateMaintainanceRequest, MaintainanceDto>,
        IRequestHandler<UpdateListMaintainanceRequest, List<MaintainanceDto>>,
        IRequestHandler<DeleteMaintainanceRequest, bool>
    {
        #region GET

        public async Task<List<MaintainanceDto>> Handle(GetMaintainanceQuery request, CancellationToken cancellationToken)
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
                      .Include(x => x.ResponsibleBy)
                      .Include(x => x.Equipment)
                      .AsNoTracking()
                      .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<MaintainanceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
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
