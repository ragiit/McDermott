using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Employee.SickLeaveCommand;

namespace McDermott.Application.Features.Queries.Employee
{
    public class SickLeaveQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetSickLeaveQuery, List<SickLeaveDto>>,
        IRequestHandler<CreateSickLeaveRequest, SickLeaveDto>,
        IRequestHandler<CreateListSickLeaveRequest, List<SickLeaveDto>>,
        IRequestHandler<UpdateSickLeaveRequest, SickLeaveDto>,
        IRequestHandler<UpdateListSickLeaveRequest, List<SickLeaveDto>>,
        IRequestHandler<DeleteSickLeaveRequest, bool>
    {
        #region GET

        public async Task<List<SickLeaveDto>> Handle(GetSickLeaveQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetSickLeaveQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<SickLeave>? result))
                {
                    result = await _unitOfWork.Repository<SickLeave>().Entities
                      .Include(x => x.GeneralConsultans)
                      .Include(x => x.GeneralConsultans.Patient)
                      //.Include(x => x.StockSickLeave)
                      .AsNoTracking()
                      .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<SickLeaveDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<SickLeaveDto> Handle(CreateSickLeaveRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<SickLeave>().AddAsync(request.SickLeaveDto.Adapt<SickLeave>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSickLeaveQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<SickLeaveDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SickLeaveDto>> Handle(CreateListSickLeaveRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<SickLeave>().AddAsync(request.SickLeaveDtos.Adapt<List<SickLeave>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSickLeaveQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<SickLeaveDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<SickLeaveDto> Handle(UpdateSickLeaveRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<SickLeave>().UpdateAsync(request.SickLeaveDto.Adapt<SickLeave>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSickLeaveQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<SickLeaveDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SickLeaveDto>> Handle(UpdateListSickLeaveRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<SickLeave>().UpdateAsync(request.SickLeaveDtos.Adapt<List<SickLeave>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSickLeaveQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<SickLeaveDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteSickLeaveRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<SickLeave>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<SickLeave>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSickLeaveQuery_"); // Ganti dengan key yang sesuai

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
