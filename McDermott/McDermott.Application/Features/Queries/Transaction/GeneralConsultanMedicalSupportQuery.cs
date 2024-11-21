using McDermott.Application.Features.Services;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class GeneralConsultanMedicalSupportHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
      IRequestHandler<GetGeneralConsultanMedicalSupportQuery, (List<GeneralConsultanMedicalSupportDto>, int pageIndex, int pageSize, int pageCount)>,
      IRequestHandler<GetSingleGeneralConsultanMedicalSupportQuery, GeneralConsultanMedicalSupportDto>,
      IRequestHandler<GetGeneralConsultanMedicalSupportLogQuery, (List<GeneralConsultanMedicalSupportLogDto>, int pageIndex, int pageSize, int pageCount)>,
      IRequestHandler<GetSingleGeneralConsultanMedicalSupportLogQuery, GeneralConsultanMedicalSupportLogDto>,
      IRequestHandler<ValidateGeneralConsultanMedicalSupport, bool>,
      IRequestHandler<GetSingleConfinedSpaceOrProcedureRoomQuery, GeneralConsultanMedicalSupportDto>,
      IRequestHandler<CreateGeneralConsultanMedicalSupportRequest, GeneralConsultanMedicalSupportDto>,
      IRequestHandler<CreateListGeneralConsultanMedicalSupportRequest, List<GeneralConsultanMedicalSupportDto>>,
      IRequestHandler<CreateGeneralConsultanMedicalSupportLogRequest, GeneralConsultanMedicalSupportLogDto>,
      IRequestHandler<CreateListGeneralConsultanMedicalSupportLogRequest, List<GeneralConsultanMedicalSupportLogDto>>,
      IRequestHandler<BulkValidateGeneralConsultanMedicalSupport, List<GeneralConsultanMedicalSupportDto>>,
      IRequestHandler<UpdateGeneralConsultanMedicalSupportRequest, GeneralConsultanMedicalSupportDto>,
      IRequestHandler<UpdateListGeneralConsultanMedicalSupportRequest, List<GeneralConsultanMedicalSupportDto>>,
      IRequestHandler<DeleteGeneralConsultanMedicalSupportRequest, bool>
    {
        #region GET

        public async Task<List<GeneralConsultanMedicalSupportDto>> Handle(BulkValidateGeneralConsultanMedicalSupport request, CancellationToken cancellationToken)
        {
            var GeneralConsultanMedicalSupportDtos = request.GeneralConsultanMedicalSupportsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            //var GeneralConsultanMedicalSupportNames = GeneralConsultanMedicalSupportDtos.Select(x => x.Name).Distinct().ToList();
            //var a = GeneralConsultanMedicalSupportDtos.Select(x => x.CountryId).Distinct().ToList();

            //var existingGeneralConsultanMedicalSupports = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>()
            //    .Entities
            //    .AsNoTracking()
            //    .Where(v => GeneralConsultanMedicalSupportNames.Contains(v.Name)
            //                && a.Contains(v.CountryId))
            //    .ToListAsync(cancellationToken);

            //return existingGeneralConsultanMedicalSupports.Adapt<List<GeneralConsultanMedicalSupportDto>>();

            return [];
        }

        public async Task<bool> Handle(ValidateGeneralConsultanMedicalSupport request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<GeneralConsultanMedicalSupport>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<GeneralConsultanMedicalSupportDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGeneralConsultanMedicalSupportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GeneralConsultanMedicalSupport>().Entities.AsNoTracking();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

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
                            ? ((IOrderedQueryable<GeneralConsultanMedicalSupport>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GeneralConsultanMedicalSupport>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    query = query.Where(v => EF.Functions.Like(v.Employee.Name, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GeneralConsultanMedicalSupport
                    {
                        Id = x.Id,
                        EmployeeId = x.EmployeeId,
                        GeneralConsultanServiceId = x.GeneralConsultanServiceId,
                        Employee = new User
                        {
                            Name = x.Employee == null ? "" : x.Employee.Name,
                        },
                        Status = x.Status,
                        IsConfinedSpace = x.IsConfinedSpace
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<GeneralConsultanMedicalSupportDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<GeneralConsultanMedicalSupportDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<GeneralConsultanMedicalSupportDto> Handle(GetSingleGeneralConsultanMedicalSupportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GeneralConsultanMedicalSupport>().Entities.AsNoTracking();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

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
                            ? ((IOrderedQueryable<GeneralConsultanMedicalSupport>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GeneralConsultanMedicalSupport>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    query = query.Where(v =>
                        EF.Functions.Like(v.Employee.Name, $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GeneralConsultanMedicalSupport
                    {
                        Id = x.Id,
                        EmployeeId = x.EmployeeId,
                        Employee = new User
                        {
                            Name = x.Employee == null ? "" : x.Employee.Name,
                        },
                        Status = x.Status,
                        IsConfinedSpace = x.IsConfinedSpace
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GeneralConsultanMedicalSupportDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<GeneralConsultanMedicalSupportDto> Handle(GetSingleConfinedSpaceOrProcedureRoomQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GeneralConsultanMedicalSupport>().Entities.AsNoTracking();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

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
                            ? ((IOrderedQueryable<GeneralConsultanMedicalSupport>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GeneralConsultanMedicalSupport>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    query = query.Where(v =>
                        EF.Functions.Like(v.Employee.Name, $"%{request.SearchTerm}%")
                        );
                }

                if (Convert.ToBoolean(query.FirstOrDefault()?.IsConfinedSpace))
                    query = query.Select(x => new GeneralConsultanMedicalSupport
                    {
                        Id = x.Id,
                        IsConfinedSpace = x.IsConfinedSpace,

                        // Confined Space fields
                        EmployeeId = x.EmployeeId,
                        IsFirstTimeEnteringConfinedSpace = x.IsFirstTimeEnteringConfinedSpace,
                        EnteringConfinedSpaceCount = x.EnteringConfinedSpaceCount,
                        IsDefectiveSenseOfSmell = x.IsDefectiveSenseOfSmell,
                        IsAsthmaOrLungAilment = x.IsAsthmaOrLungAilment,
                        IsBackPainOrLimitationOfMobility = x.IsBackPainOrLimitationOfMobility,
                        IsClaustrophobia = x.IsClaustrophobia,
                        IsDiabetesOrHypoglycemia = x.IsDiabetesOrHypoglycemia,
                        IsEyesightProblem = x.IsEyesightProblem,
                        IsFaintingSpellOrSeizureOrEpilepsy = x.IsFaintingSpellOrSeizureOrEpilepsy,
                        IsHearingDisorder = x.IsHearingDisorder,
                        IsHeartDiseaseOrDisorder = x.IsHeartDiseaseOrDisorder,
                        IsHighBloodPressure = x.IsHighBloodPressure,
                        IsLowerLimbsDeformity = x.IsLowerLimbsDeformity,
                        IsMeniereDiseaseOrVertigo = x.IsMeniereDiseaseOrVertigo,
                        RemarksMedicalHistory = x.RemarksMedicalHistory,
                        DateMedialHistory = x.DateMedialHistory,
                        SignatureEmployeeId = x.SignatureEmployeeId,
                        SignatureEmployeeImagesMedicalHistory = x.SignatureEmployeeImagesMedicalHistory,
                        SignatureEmployeeImagesMedicalHistoryBase64 = x.SignatureEmployeeImagesMedicalHistoryBase64,

                        Wt = x.Wt,
                        Bp = x.Bp,
                        Height = x.Height,
                        Pulse = x.Pulse,
                        ChestCircumference = x.ChestCircumference,
                        AbdomenCircumference = x.AbdomenCircumference,
                        RespiratoryRate = x.RespiratoryRate,
                        Temperature = x.Temperature,

                        Eye = x.Eye,
                        EarNoseThroat = x.EarNoseThroat,
                        Cardiovascular = x.Cardiovascular,
                        Respiratory = x.Respiratory,
                        Abdomen = x.Abdomen,
                        Extremities = x.Extremities,
                        Musculoskeletal = x.Musculoskeletal,
                        Neurologic = x.Neurologic,
                        SpirometryTest = x.SpirometryTest,
                        RespiratoryFitTest = x.RespiratoryFitTest,
                        Size = x.Size,
                        Comment = x.Comment,
                        Recommendeds = x.Recommendeds,
                        DateEximinedbyDoctor = x.DateEximinedbyDoctor,
                        SignatureEximinedDoctor = x.SignatureEximinedDoctor,
                        SignatureEximinedDoctorBase64 = x.SignatureEximinedDoctorBase64,
                        Recommended = x.Recommended,
                        ExaminedPhysicianId = x.ExaminedPhysicianId
                    });
                else
                    query = query.Select(x => new GeneralConsultanMedicalSupport
                    {
                        Id = x.Id,
                        IsConfinedSpace = x.IsConfinedSpace,
                        EmployeeId = x.EmployeeId,
                        Employee = new User
                        {
                            Gender = x.Employee == null ? "" : x.Employee.Gender,
                        },
                        LabTestId = x.LabTestId,
                        GeneralConsultanServiceId = x.GeneralConsultanServiceId,
                        PractitionerLabEximinationId = x.PractitionerLabEximinationId,
                        LabEximinationName = x.LabEximinationName,
                        LabResulLabExaminationtId = x.LabResulLabExaminationtId,
                        LabResulLabExaminationtIds = x.LabResulLabExaminationtIds,
                        LabEximinationAttachment = x.LabEximinationAttachment,
                        PractitionerRadiologyEximinationId = x.PractitionerRadiologyEximinationId,
                        RadiologyEximinationName = x.RadiologyEximinationName,
                        RadiologyEximinationAttachment = x.RadiologyEximinationAttachment,
                        PractitionerAlcoholEximinationId = x.PractitionerAlcoholEximinationId,
                        AlcoholEximinationName = x.AlcoholEximinationName,
                        AlcoholEximinationAttachment = x.AlcoholEximinationAttachment,
                        AlcoholNegative = x.AlcoholNegative,
                        AlcoholPositive = x.AlcoholPositive,
                        PractitionerDrugEximinationId = x.PractitionerDrugEximinationId,
                        DrugEximinationName = x.DrugEximinationName,
                        DrugEximinationAttachment = x.DrugEximinationAttachment,
                        DrugNegative = x.DrugNegative,
                        DrugPositive = x.DrugPositive,
                        AmphetaminesNegative = x.AmphetaminesNegative,
                        AmphetaminesPositive = x.AmphetaminesPositive,
                        BenzodiazepinesNegative = x.BenzodiazepinesNegative,
                        BenzodiazepinesPositive = x.BenzodiazepinesPositive,
                        CocaineMetabolitesNegative = x.CocaineMetabolitesNegative,
                        CocaineMetabolitesPositive = x.CocaineMetabolitesPositive,
                        OpiatesNegative = x.OpiatesNegative,
                        OpiatesPositive = x.OpiatesPositive,
                        MethamphetaminesNegative = x.MethamphetaminesNegative,
                        MethamphetaminesPositive = x.MethamphetaminesPositive,
                        THCCannabinoidMarijuanaNegative = x.THCCannabinoidMarijuanaNegative,
                        THCCannabinoidMarijuanaPositive = x.THCCannabinoidMarijuanaPositive,
                        OtherExaminationAttachment = x.OtherExaminationAttachment,
                        ECGAttachment = x.ECGAttachment,
                        IsOtherExaminationECG = x.IsOtherExaminationECG,
                        OtherExaminationTypeECG = x.OtherExaminationTypeECG,
                        OtherExaminationRemarkECG = x.OtherExaminationRemarkECG,
                        PractitionerECGId = x.PractitionerECGId,
                        IsNormalRestingECG = x.IsNormalRestingECG,
                        IsSinusRhythm = x.IsSinusRhythm,
                        IsSinusBradycardia = x.IsSinusBradycardia,
                        IsSinusTachycardia = x.IsSinusTachycardia,
                        HR = x.HR,
                        IsOtherECG = x.IsOtherECG,
                        OtherDesc = x.OtherDesc,
                        Status = x.Status
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GeneralConsultanMedicalSupportDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #region Get Log

        public async Task<(List<GeneralConsultanMedicalSupportLogDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGeneralConsultanMedicalSupportLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GeneralConsultanMedicalSupportLog>().Entities.AsNoTracking();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

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
                            ? ((IOrderedQueryable<GeneralConsultanMedicalSupportLog>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GeneralConsultanMedicalSupportLog>)query).ThenBy(additionalOrderBy.OrderBy);
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

                //if (!string.IsNullOrEmpty(request.SearchTerm))
                //{
                //    query = query.Where(v => EF.Functions.Like(v.CreatedDate, $"%{request.SearchTerm}%"));
                //}

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GeneralConsultanMedicalSupportLog
                    {
                        Id = x.Id,
                        UserById = x.UserById,
                        GeneralConsultanMedicalSupportId = x.GeneralConsultanMedicalSupportId,
                        UserBy = new User
                        {
                            Name = x.UserBy == null ? "" : x.UserBy.Name,
                        },
                        Status = x.Status,
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<GeneralConsultanMedicalSupportLogDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<GeneralConsultanMedicalSupportLogDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<GeneralConsultanMedicalSupportLogDto> Handle(GetSingleGeneralConsultanMedicalSupportLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GeneralConsultanMedicalSupportLog>().Entities.AsNoTracking();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

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
                            ? ((IOrderedQueryable<GeneralConsultanMedicalSupportLog>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GeneralConsultanMedicalSupportLog>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    query = query.Where(v =>
                        EF.Functions.Like(v.UserBy.Name, $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GeneralConsultanMedicalSupportLog
                    {
                        Id = x.Id,
                        UserById = x.UserById,
                        GeneralConsultanMedicalSupportId = x.GeneralConsultanMedicalSupportId,
                        UserBy = new User
                        {
                            Name = x.UserBy == null ? "" : x.UserBy.Name,
                        },
                        Status = x.Status,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GeneralConsultanMedicalSupportLogDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion Get Log

        #endregion GET

        #region CREATE

        public async Task<GeneralConsultanMedicalSupportDto> Handle(CreateGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = new GeneralConsultanMedicalSupport();

                if (request.GeneralConsultanMedicalSupportDto.IsConfinedSpace)
                {
                    var a = request.GeneralConsultanMedicalSupportDto.Adapt<CreateUpdateConfinedSpace>().Adapt<GeneralConsultanMedicalSupport>();
                    result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().AddAsync(a);
                }
                else
                {
                    var a = request.GeneralConsultanMedicalSupportDto.Adapt<CreateUpdateProcedureRoom>().Adapt<GeneralConsultanMedicalSupport>();
                    result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().AddAsync(a);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanMedicalSupport_"); // Ganti dengan key yang sesuai
                return result.Adapt<GeneralConsultanMedicalSupportDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanMedicalSupportDto>> Handle(CreateListGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().AddAsync(request.GeneralConsultanMedicalSupportDtos.Adapt<List<GeneralConsultanMedicalSupport>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanMedicalSupport_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GeneralConsultanMedicalSupportDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Create Log

        public async Task<GeneralConsultanMedicalSupportLogDto> Handle(CreateGeneralConsultanMedicalSupportLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanMedicalSupportLog>().AddAsync(request.GeneralConsultanMedicalSupportLogDto.Adapt<GeneralConsultanMedicalSupportLog>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanMedicalSupportLog_"); // Ganti dengan key yang sesuai
                return result.Adapt<GeneralConsultanMedicalSupportLogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanMedicalSupportLogDto>> Handle(CreateListGeneralConsultanMedicalSupportLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanMedicalSupportLog>().AddAsync(request.GeneralConsultanMedicalSupportLogDtos.Adapt<List<GeneralConsultanMedicalSupportLog>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanMedicalSupportLog_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GeneralConsultanMedicalSupportLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Create Log

        #endregion CREATE

        #region UPDATE

        public async Task<GeneralConsultanMedicalSupportDto> Handle(UpdateGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = new GeneralConsultanMedicalSupport();

                if (request.GeneralConsultanMedicalSupportDto.IsConfinedSpace)
                {
                    var a = request.GeneralConsultanMedicalSupportDto.Adapt<CreateUpdateConfinedSpace>().Adapt<GeneralConsultanMedicalSupport>();
                    result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().UpdateAsync(a);
                }
                else
                {
                    var a = request.GeneralConsultanMedicalSupportDto.Adapt<CreateUpdateProcedureRoom>().Adapt<GeneralConsultanMedicalSupport>();
                    result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().UpdateAsync(a);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanMedicalSupport_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanMedicalSupportDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanMedicalSupportDto>> Handle(UpdateListGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().UpdateAsync(request.GeneralConsultanMedicalSupportDtos.Adapt<List<GeneralConsultanMedicalSupport>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanMedicalSupport_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GeneralConsultanMedicalSupportDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanMedicalSupport_"); // Ganti dengan key yang sesuai

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