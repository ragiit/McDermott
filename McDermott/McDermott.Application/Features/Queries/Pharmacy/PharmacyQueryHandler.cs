using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Pharmacies.PharmacyCommand;

namespace McDermott.Application.Features.Queries.Pharmacies
{
    public class PharmacyQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetAllPharmacyQuery, List<PharmacyDto>>,//Pharmacy
        IRequestHandler<GetPharmacyQuery, (List<PharmacyDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSinglePharmacyQuery, PharmacyDto>, IRequestHandler<ValidatePharmacyQuery, bool>,
        IRequestHandler<BulkValidatePharmacyQuery, List<PharmacyDto>>,
        IRequestHandler<CreatePharmacyRequest, PharmacyDto>,
        IRequestHandler<CreateListPharmacyRequest, List<PharmacyDto>>,
        IRequestHandler<UpdatePharmacyRequest, PharmacyDto>,
        IRequestHandler<UpdateListPharmacyRequest, List<PharmacyDto>>,
        IRequestHandler<DeletePharmacyRequest, bool>,
        IRequestHandler<GetAllPharmacyLogQuery, List<PharmacyLogDto>>,//PharmacyLog
        IRequestHandler<GetPharmacyLogQuery, (List<PharmacyLogDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSinglePharmacyLogQuery, PharmacyLogDto>, IRequestHandler<ValidatePharmacyLogQuery, bool>,
        IRequestHandler<BulkValidatePharmacyLogQuery, List<PharmacyLogDto>>,
        IRequestHandler<CreatePharmacyLogRequest, PharmacyLogDto>,
        IRequestHandler<CreateListPharmacyLogRequest, List<PharmacyLogDto>>,
        IRequestHandler<UpdatePharmacyLogRequest, PharmacyLogDto>,
        IRequestHandler<UpdateListPharmacyLogRequest, List<PharmacyLogDto>>,
        IRequestHandler<DeletePharmacyLogRequest, bool>
    {
        #region Pharmacy

        #region GET Education Program

        public async Task<List<PharmacyDto>> Handle(GetAllPharmacyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllPharmacyQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Pharmacy>? result))
                {
                    result = await _unitOfWork.Repository<Pharmacy>().Entities

                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<PharmacyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PharmacyDto>> Handle(BulkValidatePharmacyQuery request, CancellationToken cancellationToken)
        {
            var PharmacyDtos = request.PharmacyToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var PharmacyNames = PharmacyDtos.Select(x => x.Patient.Name).Distinct().ToList();

            var existingPharmacys = await _unitOfWork.Repository<Pharmacy>()
                .Entities
                .AsNoTracking()
                .Where(v => PharmacyNames.Contains(v.Patient.Name))
                .ToListAsync(cancellationToken);

            return existingPharmacys.Adapt<List<PharmacyDto>>();
        }

        public async Task<bool> Handle(ValidatePharmacyQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Pharmacy>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<PharmacyDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetPharmacyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Pharmacy>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Pharmacy>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Pharmacy>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Patient.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Practitioner.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Pharmacy
                    {
                        Id = x.Id,
                        PatientId = x.PatientId,
                        PractitionerId = x.PractitionerId,
                        LocationId = x.LocationId,
                        ServiceId = x.ServiceId,
                        PaymentMethod = x.PaymentMethod,
                        MedicamentGroupId = x.MedicamentGroupId,
                        Status = x.Status,
                        ReceiptDate = x.ReceiptDate,
                        IsFarmacologi = x.IsFarmacologi,
                        IsFood = x.IsFood,
                        IsWeather = x.IsWeather,
                        Patient = new User
                        {
                            Name = x.Patient == null ? string.Empty : x.Patient.Name,
                        },
                        Practitioner = new User
                        {
                            Name = x.Practitioner == null ? string.Empty : x.Practitioner.Name,
                        },
                        Location = new Locations
                        {
                            Name = x.Location == null ? string.Empty : x.Location.Name,
                        },
                        Service = new Service
                        {
                            Name = x.Service == null ? string.Empty : x.Service.Name,
                        },
                        MedicamentGroup = new MedicamentGroup
                        {
                            Name = x.MedicamentGroup == null ? string.Empty : x.MedicamentGroup.Name,
                        }
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<PharmacyDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<PharmacyDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<PharmacyDto> Handle(GetSinglePharmacyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Pharmacy>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Pharmacy>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Pharmacy>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Patient.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Practitioner.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Pharmacy
                    {
                        Id = x.Id,
                        PatientId = x.PatientId,
                        PractitionerId = x.PractitionerId,
                        LocationId = x.LocationId,
                        ServiceId = x.ServiceId,
                        PaymentMethod = x.PaymentMethod,
                        MedicamentGroupId = x.MedicamentGroupId,
                        Status = x.Status,
                        ReceiptDate = x.ReceiptDate,
                        IsFarmacologi = x.IsFarmacologi,
                        IsFood = x.IsFood,
                        IsWeather = x.IsWeather,
                        Patient = new User
                        {
                            Name = x.Patient == null ? string.Empty : x.Patient.Name,
                        },
                        Practitioner = new User
                        {
                            Name = x.Practitioner == null ? string.Empty : x.Practitioner.Name,
                        },
                        Location = new Locations
                        {
                            Name = x.Location == null ? string.Empty : x.Location.Name,
                        },
                        Service = new Service
                        {
                            Name = x.Service == null ? string.Empty : x.Service.Name,
                        },
                        MedicamentGroup = new MedicamentGroup
                        {
                            Name = x.MedicamentGroup == null ? string.Empty : x.MedicamentGroup.Name,
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<PharmacyDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET Education Program

        #region CREATE

        public async Task<PharmacyDto> Handle(CreatePharmacyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Pharmacy>().AddAsync(request.PharmacyDto.Adapt<Pharmacy>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<PharmacyDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PharmacyDto>> Handle(CreateListPharmacyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Pharmacy>().AddAsync(request.PharmacyDtos.Adapt<List<Pharmacy>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<PharmacyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<PharmacyDto> Handle(UpdatePharmacyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Pharmacy>().UpdateAsync(request.PharmacyDto.Adapt<Pharmacy>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<PharmacyDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PharmacyDto>> Handle(UpdateListPharmacyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Pharmacy>().UpdateAsync(request.PharmacyDtos.Adapt<List<Pharmacy>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<PharmacyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeletePharmacyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Pharmacy>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Pharmacy>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE

        #endregion Pharmacy

        #region Pharmacy Log

        #region GET Education Program

        public async Task<List<PharmacyLogDto>> Handle(GetAllPharmacyLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllPharmacyLogQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<PharmacyLog>? result))
                {
                    result = await _unitOfWork.Repository<PharmacyLog>().Entities

                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<PharmacyLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PharmacyLogDto>> Handle(BulkValidatePharmacyLogQuery request, CancellationToken cancellationToken)
        {
            var PharmacyLogDtos = request.PharmacyLogToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var PharmacyLogNames = PharmacyLogDtos.Select(x => x.status).Distinct().ToList();

            var existingPharmacyLogs = await _unitOfWork.Repository<PharmacyLog>()
                .Entities
                .AsNoTracking()
                .Where(v => PharmacyLogNames.Contains(v.status))
                .ToListAsync(cancellationToken);

            return existingPharmacyLogs.Adapt<List<PharmacyLogDto>>();
        }

        public async Task<bool> Handle(ValidatePharmacyLogQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<PharmacyLog>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<PharmacyLogDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetPharmacyLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<PharmacyLog>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<PharmacyLog>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<PharmacyLog>)query).ThenBy(additionalOrderBy.OrderBy);
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
                //    query = query.Where(v =>
                //            EF.Functions.Like(v.status, $"%{request.SearchTerm}%") ||
                //            EF.Functions.Like(v.EventCategory.Name, $"%{request.SearchTerm}%")
                //            );
                //}

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new PharmacyLog
                    {
                        Id = x.Id,
                        PharmacyId = x.PharmacyId,
                        status = x.status,
                        UserById = x.UserById,
                        Pharmacy = new Pharmacy
                        {
                            PatientId = x.Pharmacy.PatientId,
                            PractitionerId = x.Pharmacy.PractitionerId,
                            Patient = new User
                            {
                                Name = x.Pharmacy.Patient == null ? string.Empty : x.Pharmacy.Patient.Name,
                            },
                            Practitioner = new User
                            {
                                Name = x.Pharmacy.Practitioner == null ? string.Empty : x.Pharmacy.Practitioner.Name,
                            },
                        }
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<PharmacyLogDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<PharmacyLogDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<PharmacyLogDto> Handle(GetSinglePharmacyLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<PharmacyLog>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<PharmacyLog>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<PharmacyLog>)query).ThenBy(additionalOrderBy.OrderBy);
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
                //    query = query.Where(v =>
                //            EF.Functions.Like(v.EventName, $"%{request.SearchTerm}%") ||
                //            EF.Functions.Like(v.EventCategory.Name, $"%{request.SearchTerm}%")
                //            );
                //}

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new PharmacyLog
                    {
                        Id = x.Id,
                        PharmacyId = x.PharmacyId,
                        status = x.status,
                        UserById = x.UserById,
                        Pharmacy = new Pharmacy
                        {
                            PatientId = x.Pharmacy.PatientId,
                            PractitionerId = x.Pharmacy.PractitionerId,
                            Patient = new User
                            {
                                Name = x.Pharmacy.Patient == null ? string.Empty : x.Pharmacy.Patient.Name,
                            },
                            Practitioner = new User
                            {
                                Name = x.Pharmacy.Practitioner == null ? string.Empty : x.Pharmacy.Practitioner.Name,
                            },
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<PharmacyLogDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET Education Program

        #region CREATE

        public async Task<PharmacyLogDto> Handle(CreatePharmacyLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<PharmacyLog>().AddAsync(request.PharmacyLogDto.Adapt<PharmacyLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<PharmacyLogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PharmacyLogDto>> Handle(CreateListPharmacyLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<PharmacyLog>().AddAsync(request.PharmacyLogDtos.Adapt<List<PharmacyLog>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<PharmacyLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<PharmacyLogDto> Handle(UpdatePharmacyLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<PharmacyLog>().UpdateAsync(request.PharmacyLogDto.Adapt<PharmacyLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<PharmacyLogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PharmacyLogDto>> Handle(UpdateListPharmacyLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<PharmacyLog>().UpdateAsync(request.PharmacyLogDtos.Adapt<List<PharmacyLog>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<PharmacyLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeletePharmacyLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<PharmacyLog>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Pharmacy>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPharmacyLogQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE

        #endregion Pharmacy Log
    }
}