using McDermott.Application.Dtos.Pharmacies;
using McDermott.Application.Features.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Pharmacies.DrugFormCommand;

namespace McDermott.Application.Features.Queries.Pharmacies
{
    public class DrugFormQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDrugFormQuery, (List<DrugFormDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetAllDrugFormQuery, List<DrugFormDto>>,
        IRequestHandler<ValidateDrugFormQuery, bool>,
        IRequestHandler<BulkValidateDrugFormQuery, List<DrugFormDto>>,
        IRequestHandler<CreateDrugFormRequest, DrugFormDto>,
        IRequestHandler<CreateListDrugFormRequest, List<DrugFormDto>>,
        IRequestHandler<UpdateDrugFormRequest, DrugFormDto>,
        IRequestHandler<UpdateListDrugFormRequest, List<DrugFormDto>>,
        IRequestHandler<DeleteDrugFormRequest, bool>
    {
        #region GET

        public async Task<List<DrugFormDto>> Handle(GetAllDrugFormQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetDrugFormQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

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

        public async Task<List<DrugFormDto>> Handle(BulkValidateDrugFormQuery request, CancellationToken cancellationToken)
        {
            var DrugFormDtos = request.DrugFormsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var DrugFormNames = DrugFormDtos.Select(x => x.Name).Distinct().ToList();
            var a = DrugFormDtos.Select(x => x.Code).Distinct().ToList();

            var existingDrugForms = await _unitOfWork.Repository<DrugForm>()
                .Entities
                .AsNoTracking()
                .Where(v => DrugFormNames.Contains(v.Name)
                            && a.Contains(v.Code))
                .ToListAsync(cancellationToken);

            return existingDrugForms.Adapt<List<DrugFormDto>>();
        }

        public async Task<(List<DrugFormDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetDrugFormQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<DrugForm>().Entities.AsNoTracking();

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                {
                    query = query.Select(request.Select);
                }

                var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                                  query,
                                  request.PageSize,
                                  request.PageIndex,
                                  q => q.OrderBy(x => x.Name), // Custom order by bisa diterapkan di sini
                                  cancellationToken);

                return (pagedItems.Adapt<List<DrugFormDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateDrugFormQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<DrugForm>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<DrugFormDto> Handle(CreateDrugFormRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugForm>().AddAsync(request.DrugFormDto.Adapt<DrugForm>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugFormQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DrugFormDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DrugFormDto>> Handle(CreateListDrugFormRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugForm>().AddAsync(request.DrugFormDtos.Adapt<List<DrugForm>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugFormQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DrugFormDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DrugFormDto> Handle(UpdateDrugFormRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugForm>().UpdateAsync(request.DrugFormDto.Adapt<DrugForm>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugFormQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DrugFormDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DrugFormDto>> Handle(UpdateListDrugFormRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugForm>().UpdateAsync(request.DrugFormDtos.Adapt<List<DrugForm>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugFormQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DrugFormDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDrugFormRequest request, CancellationToken cancellationToken)
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

                _cache.Remove("GetDrugFormQuery_"); // Ganti dengan key yang sesuai

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