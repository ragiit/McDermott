using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Pharmacy.FormDrugCommand;

namespace McDermott.Application.Features.Queries.Pharmacy
{
    public class FormDrugQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
         IRequestHandler<GetFormDrugQuery, List<FormDrugDto>>,
         IRequestHandler<CreateFormDrugRequest, FormDrugDto>,
         IRequestHandler<CreateListFormDrugRequest, List<FormDrugDto>>,
         IRequestHandler<UpdateFormDrugRequest, FormDrugDto>,
         IRequestHandler<UpdateListFormDrugRequest, List<FormDrugDto>>,
         IRequestHandler<DeleteFormDrugRequest, bool>
    {
        #region GET

        public async Task<List<FormDrugDto>> Handle(GetFormDrugQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetFormDrugQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique 

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<FormDrug>? result))
                {
                    result = await _unitOfWork.Repository<FormDrug>().GetAsync(
                        null,
                        cancellationToken: cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result?.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<FormDrugDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<FormDrugDto> Handle(CreateFormDrugRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<FormDrug>().AddAsync(request.FormDrugDto.Adapt<FormDrug>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetFormDrugQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<FormDrugDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<FormDrugDto>> Handle(CreateListFormDrugRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<FormDrug>().AddAsync(request.FormDrugDtos.Adapt<List<FormDrug>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetFormDrugQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<FormDrugDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<FormDrugDto> Handle(UpdateFormDrugRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<FormDrug>().UpdateAsync(request.FormDrugDto.Adapt<FormDrug>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetFormDrugQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<FormDrugDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<FormDrugDto>> Handle(UpdateListFormDrugRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<FormDrug>().UpdateAsync(request.FormDrugDtos.Adapt<List<FormDrug>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetFormDrugQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<FormDrugDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteFormDrugRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<FormDrug>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<FormDrug>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetFormDrugQuery_"); // Ganti dengan key yang sesuai

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
