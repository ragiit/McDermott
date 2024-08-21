using static McDermott.Application.Features.Commands.Transaction.VaccinationPlanCommand;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class VaccinationPlanQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetVaccinationPlanQuery, List<VaccinationPlanDto>>,
        IRequestHandler<CreateVaccinationPlanRequest, VaccinationPlanDto>,
        IRequestHandler<CreateListVaccinationPlanRequest, List<VaccinationPlanDto>>,
        IRequestHandler<UpdateVaccinationPlanRequest, VaccinationPlanDto>,
        IRequestHandler<UpdateListVaccinationPlanRequest, List<VaccinationPlanDto>>,
        IRequestHandler<DeleteVaccinationPlanRequest, bool>
    {
        #region GET

        public async Task<List<VaccinationPlanDto>> Handle(GetVaccinationPlanQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetVaccinationPlanQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<VaccinationPlan>? result))
                {
                    result = await _unitOfWork.Repository<VaccinationPlan>().Entities
                        .Include(x => x.Patient)
                        .Include(x => x.Physician)
                        .Include(x => x.SalesPerson)
                        .Include(x => x.Educator)
                        .Include(x => x.Product)
                        .Include(x => x.GeneralConsultanService)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result?.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<VaccinationPlanDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<VaccinationPlanDto> Handle(CreateVaccinationPlanRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.VaccinationPlanDto.Adapt<CreateUpdateVaccinationPlanDto>();

                var result = await _unitOfWork.Repository<VaccinationPlan>().AddAsync(req.Adapt<VaccinationPlan>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetVaccinationPlanQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<VaccinationPlanDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<VaccinationPlanDto>> Handle(CreateListVaccinationPlanRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.VaccinationPlanDtos.Adapt<List<CreateUpdateVaccinationPlanDto>>();

                var result = await _unitOfWork.Repository<VaccinationPlan>().AddAsync(request.Adapt<List<VaccinationPlan>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetVaccinationPlanQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<VaccinationPlanDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<VaccinationPlanDto> Handle(UpdateVaccinationPlanRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.VaccinationPlanDto.Adapt<CreateUpdateVaccinationPlanDto>();
                var result = await _unitOfWork.Repository<VaccinationPlan>().UpdateAsync(req.Adapt<VaccinationPlan>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetVaccinationPlanQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<VaccinationPlanDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<VaccinationPlanDto>> Handle(UpdateListVaccinationPlanRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.VaccinationPlanDtos.Adapt<List<CreateUpdateVaccinationPlanDto>>();
                var result = await _unitOfWork.Repository<VaccinationPlan>().UpdateAsync(req.Adapt<List<VaccinationPlan>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetVaccinationPlanQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<VaccinationPlanDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteVaccinationPlanRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<VaccinationPlan>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<VaccinationPlan>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetVaccinationPlanQuery_"); // Ganti dengan key yang sesuai

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