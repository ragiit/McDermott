using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentGroupCommand;

namespace McDermott.Application.Features.Queries.Pharmacy
{
    public class MedicamentGroupQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetMedicamentGroupQuery, (List<MedicamentGroupDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<ValidateMedicamentGroupQuery, bool>,
        IRequestHandler<BulkValidateMedicamentGroupQuery, List<MedicamentGroupDto>>,
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
          
        public async Task<List<MedicamentGroupDto>> Handle(BulkValidateMedicamentGroupQuery request, CancellationToken cancellationToken)
        {
            var MedicamentGroupDtos = request.MedicamentGroupsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var MedicamentGroupNames = MedicamentGroupDtos.Select(x => x.Name).Distinct().ToList();
            var a = MedicamentGroupDtos.Select(x => x.IsConcoction).Distinct().ToList();
            var b = MedicamentGroupDtos.Select(x => x.PhycisianId).Distinct().ToList();
            var c = MedicamentGroupDtos.Select(x => x.UoMId).Distinct().ToList();
            var d = MedicamentGroupDtos.Select(x => x.FormDrugId).Distinct().ToList();

            var existingMedicamentGroups = await _unitOfWork.Repository<MedicamentGroup>()
                .Entities
                .AsNoTracking()
                .Where(v => MedicamentGroupNames.Contains(v.Name)
                            && a.Contains(Convert.ToBoolean(v.IsConcoction))
                            && b.Contains(v.PhycisianId)
                            && c.Contains(v.UoMId)
                            && d.Contains(v.FormDrugId)
                            )
                .ToListAsync(cancellationToken);

            return existingMedicamentGroups.Adapt<List<MedicamentGroupDto>>();
        }

        public async Task<(List<MedicamentGroupDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetMedicamentGroupQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<MedicamentGroup>().Entities.AsNoTracking();

                // Apply custom order by if provided
                if (request.OrderBy is not null)
                    query = request.IsDescending ?
                        query.OrderByDescending(request.OrderBy) :
                        query.OrderBy(request.OrderBy);
                else
                    query = query.OrderBy(x => x.Name);

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
                        EF.Functions.Like(v.Phycisian.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.UoM.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.FormDrug.Name, $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new MedicamentGroup
                    {
                        Id = x.Id,
                        Name = x.Name,
                        PhycisianId = x.PhycisianId,
                        Phycisian = new User
                        {
                            Name = x.Phycisian == null ? string.Empty : x.Phycisian.Name,
                        },
                        IsConcoction = x.IsConcoction,
                        UoMId = x.UoMId,
                        UoM = new Uom
                        {
                            Name = x.UoM == null ? string.Empty : x.UoM.Name, 
                        }, 
                        FormDrugId = x.FormDrugId,
                        FormDrug = new DrugForm
                        {
                            Name = x.FormDrug == null ? string.Empty : x.FormDrug.Name, 
                        }
                    });

                // Paginate and sort
                var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                    query,
                    request.PageSize,
                    request.PageIndex,
                    cancellationToken
                );

                return (pagedItems.Adapt<List<MedicamentGroupDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        } 
        public async Task<bool> Handle(ValidateMedicamentGroupQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<MedicamentGroup>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
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
                if (request.Id > 0)
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
                if (request.Id > 0)
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