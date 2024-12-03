using McDermott.Application.Features.Services;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class GeneralConsultanCPPTQuery(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetGeneralConsultanCPPTQuery, List<GeneralConsultanCPPTDto>>,
        IRequestHandler<CheckExistingGeneralConsultanCPPTQuery, bool>,
        IRequestHandler<GetGeneralConsultanCPPTsQuery, (List<GeneralConsultanCPPTDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleGeneralConsultanCPPTsQuery, GeneralConsultanCPPTDto>,
        IRequestHandler<CreateGeneralConsultanCPPTRequest, GeneralConsultanCPPTDto>,
        IRequestHandler<CreateListGeneralConsultanCPPTRequest, List<GeneralConsultanCPPTDto>>,
        IRequestHandler<UpdateGeneralConsultanCPPTRequest, GeneralConsultanCPPTDto>,
        IRequestHandler<UpdateListGeneralConsultanCPPTRequest, List<GeneralConsultanCPPTDto>>,
        IRequestHandler<DeleteGeneralConsultanCPPTRequest, bool>
    {
        #region GET
        public async Task<bool> Handle(CheckExistingGeneralConsultanCPPTQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<GeneralConsultanCPPT>().Entities.AsNoTracking().AnyAsync(request.Predicate);
        }
        public async Task<(List<GeneralConsultanCPPTDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGeneralConsultanCPPTsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GeneralConsultanCPPT>().Entities.AsNoTracking();

                //// Apply custom order by if provided
                //if (request.OrderBy is not null)
                //    query = request.IsDescending ?
                //        query.OrderByDescending(request.OrderBy) :
                //        query.OrderBy(request.OrderBy);
                //else
                //    query = query.OrderBy(x => x.Id);

                // Apply ordering
                if (request.OrderByList.Count != 0)
                {
                    var firstOrderBy = request.OrderByList.First();
                    query = firstOrderBy.IsDescending
                        ? query.OrderByDescending(firstOrderBy.OrderBy)
                        : query.OrderBy(firstOrderBy.OrderBy);

                    foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                    {
                        query = additionalOrderBy.IsDescending
                            ? ((IOrderedQueryable<GeneralConsultanCPPT>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GeneralConsultanCPPT>)query).ThenBy(additionalOrderBy.OrderBy);
                    }
                }

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
                    //query = query.Where(v =>
                    //    EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.Phycisian.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.UoM.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.FormDrug.Name, $"%{request.SearchTerm}%")
                    //    );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GeneralConsultanCPPT
                    {
                        Id = x.Id,
                        Subjective = x.Subjective,
                        Objective = x.Objective,
                        NursingDiagnosesId = x.NursingDiagnosesId,
                        NursingDiagnoses = new NursingDiagnoses
                        {
                            Problem = x.NursingDiagnoses != null ? x.NursingDiagnoses.Problem : "",
                        },
                        DiagnosisId = x.DiagnosisId,
                        Diagnosis = new Diagnosis
                        {
                            Name = x.Diagnosis != null ? x.Diagnosis.Name : "",
                        },
                        UserId = x.UserId,
                        User = new User
                        {
                            Name = x.User != null ? x.User.Name : "",
                        },
                        Planning = x.Planning,
                        MedicationTherapy = x.MedicationTherapy,
                        NonMedicationTherapy = x.NonMedicationTherapy,
                        Anamnesa = x.Anamnesa,
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<GeneralConsultanCPPTDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<GeneralConsultanCPPTDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<GeneralConsultanCPPTDto> Handle(GetSingleGeneralConsultanCPPTsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GeneralConsultanCPPT>().Entities.AsNoTracking();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                //// Apply custom order by if provided
                //if (request.OrderBy is not null)
                //    query = request.IsDescending ?
                //        query.OrderByDescending(request.OrderBy) :
                //        query.OrderBy(request.OrderBy);
                //else
                //    query = query.OrderBy(x => x.Id);

                // Apply ordering
                if (request.OrderByList.Count != 0)
                {
                    var firstOrderBy = request.OrderByList.First();
                    query = firstOrderBy.IsDescending
                        ? query.OrderByDescending(firstOrderBy.OrderBy)
                        : query.OrderBy(firstOrderBy.OrderBy);

                    foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                    {
                        query = additionalOrderBy.IsDescending
                            ? ((IOrderedQueryable<GeneralConsultanCPPT>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GeneralConsultanCPPT>)query).ThenBy(additionalOrderBy.OrderBy);
                    }
                }

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    //query = query.Where(v =>
                    //    EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.Phycisian.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.UoM.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.FormDrug.Name, $"%{request.SearchTerm}%")
                    //    );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GeneralConsultanCPPT
                    {
                        Id = x.Id,
                        Subjective = x.Subjective,
                        Objective = x.Objective,
                        NursingDiagnosesId = x.NursingDiagnosesId,
                        DateTime = x.DateTime,
                        NursingDiagnoses = new NursingDiagnoses
                        {
                            Problem = x.NursingDiagnoses != null ? x.NursingDiagnoses.Problem : "",
                        },
                        DiagnosisId = x.DiagnosisId,
                        Diagnosis = new Diagnosis
                        {
                            Name = x.Diagnosis != null ? x.Diagnosis.Name : "",
                        },
                        Planning = x.Planning,
                        MedicationTherapy = x.MedicationTherapy,
                        NonMedicationTherapy = x.NonMedicationTherapy,
                        Anamnesa = x.Anamnesa,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GeneralConsultanCPPTDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<List<GeneralConsultanCPPTDto>> Handle(GetGeneralConsultanCPPTQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetGeneralConsultanCPPTQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<GeneralConsultanCPPT>? result))
                {
                    result = await _unitOfWork.Repository<GeneralConsultanCPPT>().GetAsync(
                        null,
                        x => x.Include(z => z.GeneralConsultanService),
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<GeneralConsultanCPPTDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<GeneralConsultanCPPTDto> Handle(CreateGeneralConsultanCPPTRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanCPPT>().AddAsync(request.GeneralConsultanCPPTDto.Adapt<GeneralConsultanCPPT>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanCPPTQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanCPPTDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanCPPTDto>> Handle(CreateListGeneralConsultanCPPTRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanCPPT>().AddAsync(request.GeneralConsultanCPPTDtos.Adapt<List<GeneralConsultanCPPT>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanCPPTQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GeneralConsultanCPPTDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<GeneralConsultanCPPTDto> Handle(UpdateGeneralConsultanCPPTRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanCPPT>().UpdateAsync(request.GeneralConsultanCPPTDto.Adapt<CreateUpdateGeneralConsultanCPPTDto>().Adapt<GeneralConsultanCPPT>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanCPPTQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanCPPTDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanCPPTDto>> Handle(UpdateListGeneralConsultanCPPTRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanCPPT>().UpdateAsync(request.GeneralConsultanCPPTDtos.Adapt<List<GeneralConsultanCPPT>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanCPPTQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GeneralConsultanCPPTDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteGeneralConsultanCPPTRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.DeleteByGeneralServiceId != 0)
                {
                    var list = await _unitOfWork.Repository<GeneralConsultanCPPT>().Entities.Where(x => x.GeneralConsultanServiceId == request.DeleteByGeneralServiceId).ToListAsync(cancellationToken);

                    await _unitOfWork.Repository<GeneralConsultanCPPT>().DeleteAsync(x => list.Select(z => z.Id).Contains(x.Id));
                }
                else
                {
                    if (request.Id > 0)
                    {
                        await _unitOfWork.Repository<GeneralConsultanCPPT>().DeleteAsync(request.Id);
                    }

                    if (request.Ids.Count > 0)
                    {
                        await _unitOfWork.Repository<GeneralConsultanCPPT>().DeleteAsync(x => request.Ids.Contains(x.Id));
                    }
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanCPPTQuery_"); // Ganti dengan key yang sesuai

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