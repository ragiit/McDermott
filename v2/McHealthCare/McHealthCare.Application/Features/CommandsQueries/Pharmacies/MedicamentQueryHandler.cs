﻿using static McHealthCare.Application.Features.Commands.Pharmacies.MedicamentCommand;

namespace McHealthCare.Application.Features.Queries.Pharmacy
{
    public class MedicamentQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
         IRequestHandler<GetMedicamentQuery, List<MedicamentDto>>,
         IRequestHandler<CreateMedicamentRequest, MedicamentDto>,
         IRequestHandler<CreateListMedicamentRequest, List<MedicamentDto>>,
         IRequestHandler<UpdateMedicamentRequest, MedicamentDto>,
         IRequestHandler<UpdateListMedicamentRequest, List<MedicamentDto>>,
         IRequestHandler<DeleteMedicamentRequest, bool>
    {
        #region GET

        public async Task<List<MedicamentDto>> Handle(GetMedicamentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetMedicamentQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Medicament>? result))
                {
                    result = await _unitOfWork.Repository<Medicament>().Entities
                        .Include(x => x.Uom)
                        .Include(x => x.Product)
                        .Include(x => x.Frequency)
                        .Include(x => x.Route)
                        .Include(x => x.ActiveComponent)
                      .AsNoTracking()
                      .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<MedicamentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<MedicamentDto> Handle(CreateMedicamentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Medicament>().AddAsync(request.MedicamentDto.Adapt<Medicament>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MedicamentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MedicamentDto>> Handle(CreateListMedicamentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Medicament>().AddAsync(request.MedicamentDtos.Adapt<List<Medicament>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MedicamentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<MedicamentDto> Handle(UpdateMedicamentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Medicament>().UpdateAsync(request.MedicamentDto.Adapt<Medicament>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MedicamentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MedicamentDto>> Handle(UpdateListMedicamentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Medicament>().UpdateAsync(request.MedicamentDtos.Adapt<List<Medicament>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MedicamentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteMedicamentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty)
                {
                    await _unitOfWork.Repository<Medicament>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Medicament>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentQuery_"); // Ganti dengan key yang sesuai
                _cache.Remove("GetUomQuery_");

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