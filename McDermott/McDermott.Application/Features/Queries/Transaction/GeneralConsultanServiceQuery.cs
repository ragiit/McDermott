using Microsoft.EntityFrameworkCore;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class GeneralConsultanServiceQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetGeneralConsultanServiceQuery, List<GeneralConsultanServiceDto>>,
        IRequestHandler<CreateGeneralConsultanServiceRequest, GeneralConsultanServiceDto>,
        IRequestHandler<CreateListGeneralConsultanServiceRequest, List<GeneralConsultanServiceDto>>,
        IRequestHandler<UpdateGeneralConsultanServiceRequest, GeneralConsultanServiceDto>,
        IRequestHandler<UpdateStatusGeneralConsultanServiceRequest, GeneralConsultanServiceDto>,
        IRequestHandler<DeleteGeneralConsultanServiceRequest, bool>,
        IRequestHandler<GetGeneralConsultationLogQuery, List<GeneralConsultanlogDto>>,
        IRequestHandler<CreateGeneralConsultationLogRequest, GeneralConsultanlogDto>,
        IRequestHandler<CreateListGeneralConsultationLogRequest, List<GeneralConsultanlogDto>>,
        IRequestHandler<UpdateGeneralConsultationLogRequest, GeneralConsultanlogDto>,
        IRequestHandler<DeleteGeneralConsultationLogRequest, bool>
    {
        #region GET

        public async Task<List<GeneralConsultanServiceDto>> Handle(GetGeneralConsultanServiceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetGeneralConsultanServiceQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<GeneralConsultanService>? result))
                {
                    result = await _unitOfWork.Repository<GeneralConsultanService>().Entities
                        .Include(z => z.Service)
                        .Include(z => z.Pratitioner)
                        .Include(z => z.ClassType)
                        .Include(z => z.InsurancePolicy)
                        .Include(z => z.Patient)
                        .Include(z => z.Patient!.Gender)
                        .Include(z => z.Patient.Department)
                        .Include(z => z.Patient.IdCardCountry)
                        //.ThenInclude(z => z.Gender)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<GeneralConsultanServiceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<GeneralConsultanServiceDto> Handle(CreateGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.GeneralConsultanServiceDto.Adapt<CreateUpdateGeneralConsultanServiceDto>();
                var result = await _unitOfWork.Repository<GeneralConsultanService>().AddAsync(req.Adapt<GeneralConsultanService>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_");

                return result.Adapt<GeneralConsultanServiceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanServiceDto>> Handle(CreateListGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.GeneralConsultanServiceDtos.Adapt<CreateUpdateGeneralConsultanServiceDto>();
                var result = await _unitOfWork.Repository<GeneralConsultanService>().AddAsync(req.Adapt<List<GeneralConsultanService>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_");

                return result.Adapt<List<GeneralConsultanServiceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region Update

        public async Task<GeneralConsultanServiceDto> Handle(UpdateStatusGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanService>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id) ?? new();

                result.Status = request.Status;

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanServiceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GeneralConsultanServiceDto> Handle(UpdateGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.GeneralConsultanServiceDto.Adapt<CreateUpdateGeneralConsultanServiceDto>();
                var result = await _unitOfWork.Repository<GeneralConsultanService>().UpdateAsync(req.Adapt<GeneralConsultanService>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanServiceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Update

        #region Delete

        public async Task<bool> Handle(DeleteGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanService>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanService>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Delete

        #region GET General Consultan Service Log

        public async Task<List<GeneralConsultanlogDto>> Handle(GetGeneralConsultationLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetGeneralConsultationLogQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<GeneralConsultationLog>? result))
                {
                    result = await _unitOfWork.Repository<GeneralConsultationLog>().Entities
                        .Include(z => z.GeneralConsultanService)
                        .Include(z => z.ProcedureRoom)
                        .Include(z => z.UserBy)

                        //.ThenInclude(z => z.Gender)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<GeneralConsultanlogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET General Consultan Service Log

        #region CREATE General Consultan Service Log

        public async Task<GeneralConsultanlogDto> Handle(CreateGeneralConsultationLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultationLog>().AddAsync(request.GeneralConsultanlogDto.Adapt<GeneralConsultationLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultationLogQuery_");

                return result.Adapt<GeneralConsultanlogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanlogDto>> Handle(CreateListGeneralConsultationLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultationLog>().AddAsync(request.GeneralConsultanlogDto.Adapt<List<GeneralConsultationLog>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultationLogQuery_");

                return result.Adapt<List<GeneralConsultanlogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE General Consultan Service Log

        #region Update General Consultan Service Log

        public async Task<GeneralConsultanlogDto> Handle(UpdateGeneralConsultationLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultationLog>().UpdateAsync(request.GeneralConsultanlogDto.Adapt<GeneralConsultationLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultationLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanlogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Update General Consultan Service Log

        #region Delete General Consultan Service Log

        public async Task<bool> Handle(DeleteGeneralConsultationLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultationLog>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultationLog>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultationLogQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Delete General Consultan Service Log
    }
}