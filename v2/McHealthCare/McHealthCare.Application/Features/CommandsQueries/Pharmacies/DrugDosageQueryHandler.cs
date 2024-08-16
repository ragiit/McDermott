using static McHealthCare.Application.Features.Commands.Pharmacies.DrugDosageCommand;

namespace McHealthCare.Application.Features.Queries.Pharmacy
{
    public class DrugDosageQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDrugDosageQuery, List<DrugDosageDto>>,
        IRequestHandler<CreateDrugDosageRequest, DrugDosageDto>,
        IRequestHandler<CreateListDrugDosageRequest, List<DrugDosageDto>>,
        IRequestHandler<UpdateDrugDosageRequest, DrugDosageDto>,
        IRequestHandler<UpdateListDrugDosageRequest, List<DrugDosageDto>>,
        IRequestHandler<DeleteDrugDosageRequest, bool>
    {
        #region GET

        public async Task<List<DrugDosageDto>> Handle(GetDrugDosageQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetDrugDosageQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<DrugDosage>? result))
                {
                    result = await _unitOfWork.Repository<DrugDosage>().Entities
                        .Include(x => x.DrugRoute)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<DrugDosageDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<DrugDosageDto> Handle(CreateDrugDosageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugDosage>().AddAsync(request.DrugDosageDto.Adapt<DrugDosage>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugDosageQuery_");

                return result.Adapt<DrugDosageDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DrugDosageDto>> Handle(CreateListDrugDosageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugDosage>().AddAsync(request.DrugDosageDtos.Adapt<List<DrugDosage>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugDosageQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DrugDosageDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DrugDosageDto> Handle(UpdateDrugDosageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugDosage>().UpdateAsync(request.DrugDosageDto.Adapt<DrugDosage>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugDosageQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DrugDosageDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DrugDosageDto>> Handle(UpdateListDrugDosageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugDosage>().UpdateAsync(request.DrugDosageDtos.Adapt<List<DrugDosage>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugDosageQuery_");

                return result.Adapt<List<DrugDosageDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDrugDosageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty)
                {
                    await _unitOfWork.Repository<DrugDosage>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<DrugDosage>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugDosageQuery_");

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