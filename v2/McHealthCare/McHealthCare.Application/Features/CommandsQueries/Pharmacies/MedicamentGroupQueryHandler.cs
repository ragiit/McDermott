using static McHealthCare.Application.Features.Commands.Pharmacies.MedicamentGroupCommand;

namespace McHealthCare.Application.Features.Queries.Pharmacy
{
    public class MedicamentGroupQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetMedicamentGroupQuery, List<MedicamentGroupDto>>,
        IRequestHandler<CreateMedicamentGroupRequest, MedicamentGroupDto>,
        IRequestHandler<CreateListMedicamentGroupRequest, List<MedicamentGroupDto>>,
        IRequestHandler<UpdateMedicamentGroupRequest, MedicamentGroupDto>,
        IRequestHandler<UpdateListMedicamentGroupRequest, List<MedicamentGroupDto>>,
        IRequestHandler<DeleteMedicamentGroupRequest, bool>,
        IRequestHandler<GetMedicamentGroupDetailQuery, List<MedicamentGroupDetailDto>>,
        IRequestHandler<CreateMedicamentGroupDetailRequest, MedicamentGroupDetailDto>,
        IRequestHandler<CreateListMedicamentGroupDetailRequest, List<MedicamentGroupDetailDto>>,
        IRequestHandler<UpdateMedicamentGroupDetailRequest, MedicamentGroupDetailDto>,
        IRequestHandler<UpdateListMedicamentGroupDetailRequest, List<MedicamentGroupDetailDto>>,
        IRequestHandler<DeleteMedicamentGroupDetailRequest, bool>
    {
        #region GET

        public async Task<List<MedicamentGroupDto>> Handle(GetMedicamentGroupQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetMedicamentGroupQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<MedicamentGroup>? result))
                {
                    result = await _unitOfWork.Repository<MedicamentGroup>().Entities
                      .Include(x => x.Phycisian)
                      .Include(x => x.UoM)
                      .Include(x => x.FormDrug)
                      .AsNoTracking()
                      .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<MedicamentGroupDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MedicamentGroupDetailDto>> Handle(GetMedicamentGroupDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetMedicamentGroupDetailQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<MedicamentGroupDetail>? result))
                {
                    result = await _unitOfWork.Repository<MedicamentGroupDetail>().Entities
                        .Include(x => x.ActiveComponent)
                        .Include(x => x.Medicament)
                        .Include(x => x.MedicamentGroup)
                        .Include(x => x.Frequency)
                        .Include(x => x.UnitOfDosage)
                          .AsNoTracking()
                          .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<MedicamentGroupDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<MedicamentGroupDto> Handle(CreateMedicamentGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MedicamentGroup>().AddAsync(request.MedicamentGroupDto.Adapt<MedicamentGroup>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentGroupQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MedicamentGroupDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MedicamentGroupDto>> Handle(CreateListMedicamentGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MedicamentGroup>().AddAsync(request.MedicamentGroupDtos.Adapt<List<MedicamentGroup>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentGroupQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MedicamentGroupDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MedicamentGroupDetailDto> Handle(CreateMedicamentGroupDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MedicamentGroupDetail>().AddAsync(request.MedicamentGroupDetailDto.Adapt<MedicamentGroupDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentGroupDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MedicamentGroupDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MedicamentGroupDetailDto>> Handle(CreateListMedicamentGroupDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MedicamentGroupDetail>().AddAsync(request.MedicamentGroupDetailDtos.Adapt<List<MedicamentGroupDetail>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentGroupDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MedicamentGroupDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<MedicamentGroupDto> Handle(UpdateMedicamentGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MedicamentGroup>().UpdateAsync(request.MedicamentGroupDto.Adapt<MedicamentGroup>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentGroupQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MedicamentGroupDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MedicamentGroupDto>> Handle(UpdateListMedicamentGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MedicamentGroup>().UpdateAsync(request.MedicamentGroupDtos.Adapt<List<MedicamentGroup>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentGroupQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MedicamentGroupDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MedicamentGroupDetailDto> Handle(UpdateMedicamentGroupDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MedicamentGroupDetail>().UpdateAsync(request.MedicamentGroupDetailDto.Adapt<MedicamentGroupDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentGroupDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MedicamentGroupDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MedicamentGroupDetailDto>> Handle(UpdateListMedicamentGroupDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MedicamentGroupDetail>().UpdateAsync(request.MedicamentGroupDetailDtos.Adapt<List<MedicamentGroupDetail>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentGroupDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MedicamentGroupDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteMedicamentGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty)
                {
                    await _unitOfWork.Repository<MedicamentGroup>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<MedicamentGroup>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentGroupQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(DeleteMedicamentGroupDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty)
                {
                    await _unitOfWork.Repository<MedicamentGroupDetail>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<MedicamentGroupDetail>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentGroupDetailQuery_"); // Ganti dengan key yang sesuai

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