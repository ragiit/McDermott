using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Pharmacy.FormDrugCommand;

namespace McDermott.Application.Features.Queries.Pharmacy
{
    public class FormDrugQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
         IRequestHandler<GetFormDrugQuery, List<DrugFormDto>>,
         IRequestHandler<CreateFormDrugRequest, DrugFormDto>,
         IRequestHandler<CreateListFormDrugRequest, List<DrugFormDto>>,
         IRequestHandler<UpdateFormDrugRequest, DrugFormDto>,
         IRequestHandler<UpdateListFormDrugRequest, List<DrugFormDto>>,
         IRequestHandler<DeleteFormDrugRequest, bool>
    {
        #region GET

        public async Task<List<DrugFormDto>> Handle(GetFormDrugQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetFormDrugQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique 

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<DrugForm>? result))
                {
                    result = await _unitOfWork.Repository<DrugForm>().GetAsync(
                        null,
                        cancellationToken: cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result?.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<DrugFormDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<DrugFormDto> Handle(CreateFormDrugRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugForm>().AddAsync(request.FormDrugDto.Adapt<DrugForm>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetFormDrugQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DrugFormDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DrugFormDto>> Handle(CreateListFormDrugRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugForm>().AddAsync(request.FormDrugDtos.Adapt<List<DrugForm>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetFormDrugQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DrugFormDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DrugFormDto> Handle(UpdateFormDrugRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugForm>().UpdateAsync(request.FormDrugDto.Adapt<DrugForm>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetFormDrugQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DrugFormDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DrugFormDto>> Handle(UpdateListFormDrugRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugForm>().UpdateAsync(request.FormDrugDtos.Adapt<List<DrugForm>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetFormDrugQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DrugFormDto>>();
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
                    await _unitOfWork.Repository<DrugForm>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<DrugForm>().DeleteAsync(x => request.Ids.Contains(x.Id));
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
