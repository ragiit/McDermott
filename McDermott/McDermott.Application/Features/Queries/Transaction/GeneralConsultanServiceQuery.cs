using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using McDermott.Application.Extentions;
using McDermott.Application.Features.Services;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Linq;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class GeneralConsultanServiceQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetGeneralConsultanServiceQuery, (List<GeneralConsultanServiceDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetGeneralConsultanServicesQuery, (List<GeneralConsultanServiceDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateGeneralConsultanServiceRequest, GeneralConsultanServiceDto>,
        IRequestHandler<CreateListGeneralConsultanServiceRequest, List<GeneralConsultanServiceDto>>,
        IRequestHandler<UpdateGeneralConsultanServiceRequest, GeneralConsultanServiceDto>,
        IRequestHandler<CancelGeneralConsultanServiceRequest, GeneralConsultanServiceDto>,
        IRequestHandler<GetSingleGeneralConsultanServicesQuery, GeneralConsultanServiceDto>,
        IRequestHandler<UpdateStatusGeneralConsultanServiceRequest, GeneralConsultanServiceDto>,
        IRequestHandler<DeleteGeneralConsultanServiceRequest, bool>,
        IRequestHandler<GetGeneralConsultationServiceLogQuery, List<GeneralConsultationServiceLogDto>>,
        IRequestHandler<CreateGeneralConsultationLogRequest, GeneralConsultationServiceLogDto>,
        IRequestHandler<CreateListGeneralConsultationLogRequest, List<GeneralConsultationServiceLogDto>>,
        IRequestHandler<UpdateGeneralConsultationLogRequest, GeneralConsultationServiceLogDto>,
        IRequestHandler<DeleteGeneralConsultationLogRequest, bool>,
        IRequestHandler<GetGeneralConsultanServiceCountQuery, int>,

        IRequestHandler<CreateFormGeneralConsultanServiceNewRequest, GeneralConsultanServiceDto>,
        IRequestHandler<UpdateFormGeneralConsultanServiceNewRequest, GeneralConsultanServiceDto>,
        IRequestHandler<UpdateConfirmFormGeneralConsultanServiceNewRequest, GeneralConsultanServiceDto>
    {
        #region GET

        public async Task<GeneralConsultanServiceDto> Handle(GetSingleGeneralConsultanServicesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GeneralConsultanService>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<GeneralConsultanService>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GeneralConsultanService>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    query = query.Select(x => new GeneralConsultanService
                    {
                        Id = x.Id,
                        Status = x.Status,
                        PatientId = x.PatientId,
                        Patient = new User
                        {
                            Name = x.Patient == null ? string.Empty : x.Patient.Name,
                        },
                        PratitionerId = x.PratitionerId,
                        Pratitioner = new User
                        {
                            Name = x.Pratitioner == null ? string.Empty : x.Pratitioner.Name,
                        },
                        ServiceId = x.ServiceId,
                        Service = new Service
                        {
                            Name = x.Service == null ? string.Empty : x.Service.Name,
                        },
                        Payment = x.Payment,
                        AppointmentDate = x.AppointmentDate,
                        IsAlertInformationSpecialCase = x.IsAlertInformationSpecialCase,
                        RegistrationDate = x.RegistrationDate,
                        ClassType = x.ClassType,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GeneralConsultanServiceDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<(List<GeneralConsultanServiceDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGeneralConsultanServicesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GeneralConsultanService>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<GeneralConsultanService>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GeneralConsultanService>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //    EF.Functions.Like(v.Reference, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.SerialNo, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.Status.GetDisplayName(), $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.StatusMCU.GetDisplayName(), $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.Patient.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.Pratitioner.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.RegistrationDate.Value.ToString("dd-MM-yyyy"), $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.AppointmentDate.Value.ToString("dd-MM-yyyy"), $"%{request.SearchTerm}%"));

                    var result = EnumHelper.GetEnumByDisplayName<EnumStatusGeneralConsultantService>(request.SearchTerm);
                    var resultMcu = EnumHelper.GetEnumByDisplayName<EnumStatusMCU>(request.SearchTerm);

                    DateTime parsedDate;
                    bool isDate = DateTime.TryParseExact(request.SearchTerm, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate);

                    query = query
                        .Where(v =>
                            EF.Functions.Like(v.Reference, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.SerialNo, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Patient.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Pratitioner.Name, $"%{request.SearchTerm}%") ||
                            (result != null && v.Status == result) ||
                            (resultMcu != null && v.StatusMCU == resultMcu) ||
                                (isDate && v.RegistrationDate.HasValue && v.RegistrationDate.Value.Date == parsedDate.Date)

                            //v.AppointmentDate.HasValue && v.AppointmentDate.Value.ToString("dd-MM-yyyy").Contains(request.SearchTerm
                            );

                    //.AsEnumerable()  // Evaluasi di memori mulai dari sini
                    //.Where(v =>
                    //    v.Status.GetDisplayName().ToLower().Contains(request.SearchTerm) ||
                    //    v.StatusMCU.GetDisplayName().ToLower().Contains(request.SearchTerm) ||
                    //    v.RegistrationDate.HasValue && v.RegistrationDate.Value.ToString("dd-MM-yyyy").Contains(request.SearchTerm) ||
                    //    v.AppointmentDate.HasValue && v.AppointmentDate.Value.ToString("dd-MM-yyyy").Contains(request.SearchTerm))
                    //.AsQueryable();
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GeneralConsultanService
                    {
                        Id = x.Id,
                        Status = x.Status,
                        PatientId = x.PatientId,
                        Patient = new User
                        {
                            Name = x.Patient == null ? string.Empty : x.Patient.Name,
                        },
                        PratitionerId = x.PratitionerId,
                        Pratitioner = new User
                        {
                            Name = x.Pratitioner == null ? string.Empty : x.Pratitioner.Name,
                        },
                        ServiceId = x.ServiceId,
                        Service = new Service
                        {
                            Name = x.Service == null ? string.Empty : x.Service.Name,
                            IsMaternity = x.IsMaternity
                        },
                        Payment = x.Payment,
                        AppointmentDate = x.AppointmentDate,
                        IsAlertInformationSpecialCase = x.IsAlertInformationSpecialCase,
                        RegistrationDate = x.RegistrationDate,
                        TypeRegistration = x.TypeRegistration,
                        ClassType = x.ClassType,
                        SerialNo = x.SerialNo,
                        Reference = x.Reference,
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<GeneralConsultanServiceDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<GeneralConsultanServiceDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<int> Handle(GetGeneralConsultanServiceCountQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<GeneralConsultanService>().Entities
                .Where(request.Predicate)
                .Include(x => x.Service)
                .AsNoTracking()
                .CountAsync(cancellationToken: cancellationToken);
        }

        public async Task<(List<GeneralConsultanServiceDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGeneralConsultanServiceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GeneralConsultanService>().Entities
                    .AsNoTracking()
                    .Include(z => z.Service)
                    .Include(z => z.Pratitioner)
                    .Include(z => z.InsurancePolicy)
                    .Include(z => z.Patient)
                    .Include(z => z.Patient.Department)
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    //query = query.Where(v =>);
                }

                //var pagedResult = query
                //            .OrderBy(x => x.Name);

                var totalCount = await query.CountAsync(cancellationToken);

                var skip = (request.PageIndex) * request.PageSize;

                var paged = query
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<GeneralConsultanServiceDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<GeneralConsultanServiceDto> Handle(CreateGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.GeneralConsultanServiceDto.Adapt<CreateUpdateGeneralConsultanServiceDto>();
                var result = await _unitOfWork.Repository<GeneralConsultanService>().AddAsync(req.Adapt<GeneralConsultanService>());

                var currentDate = DateTime.Now;
                string year = currentDate.ToString("yyyy");
                string month = currentDate.ToString("MM");

                var lastTransaction = await _unitOfWork.Repository<GeneralConsultanService>().Entities
                    .Where(x => x.CreatedDate.Value.Year == currentDate.Year
                && x.CreatedDate.Value.Month == currentDate.Month).OrderByDescending(x => x.Id).Select(x => x.Reference).FirstOrDefaultAsync();

                int newSequence = 1;

                if (lastTransaction != null)
                {
                    string lastSequenceNumber = lastTransaction;

                    // Pastikan lastSequenceNumber memiliki format yang benar
                    if (lastSequenceNumber.Length == 16 && lastSequenceNumber.StartsWith("GC/"))
                    {
                        // Extract the sequence part (XXXXX) from the last sequence number
                        string sequencePart = lastSequenceNumber.Substring(11, 5);

                        if (int.TryParse(sequencePart, out int lastSequence))
                        {
                            // Increment sequence number
                            newSequence = lastSequence + 1;
                        }
                        else
                        {
                            // Handle the case where the sequence part is not a valid number
                            throw new FormatException($"The sequence part '{sequencePart}' is not a valid number.");
                        }
                    }
                    else
                    {
                        // Handle the case where lastSequenceNumber does not match expected format
                        throw new FormatException($"The last sequence number '{lastSequenceNumber}' does not match the expected format 'GC/YYYY/MM/XXXXX'.");
                    }
                }

                // Generate the new sequence number with format GC/YYYY/MM/XXXXX
                string sequenceNumber = $"GC/{year}/{month}/{newSequence:D5}";

                result.Reference = sequenceNumber;

                // Generate the new sequence number with format GC/YYYY/MM/XXXXX
                //string sequenceNumber = $"GC/{year}/{month}/{newSequence:D5}";

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_");

                return result.Adapt<GeneralConsultanServiceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanServiceDto>> Handle(CreateListGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.GeneralConsultanServiceDtos.Adapt<CreateUpdateGeneralConsultanServiceDto>();
                var result = await _unitOfWork.Repository<GeneralConsultanService>().AddAsync(req.Adapt<List<GeneralConsultanService>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_");

                return result.Adapt<List<GeneralConsultanServiceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region Update

        public async Task<GeneralConsultanServiceDto> Handle(UpdateStatusGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanService>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id) ?? new();

                if (result is null)
                    return new();

                _unitOfWork.Repository<GeneralConsultanService>().Attach(result);

                result.Status = request.Status;

                // Mark specific properties as modified
                _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(result, nameof(result.Status));

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanServiceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GenerateGCSequenceNumberAsync(List<GeneralConsultanServiceDto> generalConsultanServices)
        {
            var currentDate = DateTime.Now;
            string year = currentDate.ToString("yyyy");
            string month = currentDate.ToString("MM");

            // Ambil transaksi terakhir di bulan dan tahun ini
            //var lastTransaction = await _context.Transactions
            //    .Where(t => t.ConfirmDate.Year == currentDate.Year && t.ConfirmDate.Month == currentDate.Month)
            //    .OrderByDescending(t => t.Id)
            //    .FirstOrDefaultAsync();

            var lastTransaction = generalConsultanServices.Where(x => x.CreatedDate.GetValueOrDefault().Year == currentDate.Year
            && x.CreatedDate.GetValueOrDefault().Month == currentDate.Month).OrderByDescending(x => x.Id).FirstOrDefault();

            int newSequence = 1;

            if (lastTransaction != null)
            {
                // Extract the sequence part (XXXXX) from the last sequence number
                string lastSequenceNumber = lastTransaction.Reference;
                int lastSequence = int.Parse(lastSequenceNumber.Substring(10, 5));

                // Increment sequence number
                newSequence = lastSequence + 1;
            }

            // Generate the new sequence number with format GC/YYYY/MM/XXXXX
            string sequenceNumber = $"GC/{year}/{month}/{newSequence:D5}";

            return sequenceNumber;
        }

        public async Task<GeneralConsultanServiceDto> Handle(UpdateGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.GeneralConsultanServiceDto.Adapt<CreateUpdateGeneralConsultanServiceDto>();
                var result = await _unitOfWork.Repository<GeneralConsultanService>().UpdateAsync(req.Adapt<GeneralConsultanService>());

                if (string.IsNullOrWhiteSpace(result.Reference))
                {
                    var currentDate = DateTime.Now;
                    string year = currentDate.ToString("yyyy");
                    string month = currentDate.ToString("MM");

                    var lastTransaction = await _unitOfWork.Repository<GeneralConsultanService>().Entities
                        .Where(x => x.CreatedDate.Value.Year == currentDate.Year
                    && x.CreatedDate.Value.Month == currentDate.Month).OrderByDescending(x => x.Id).Select(x => x.Reference).FirstOrDefaultAsync();

                    int newSequence = 1;

                    if (lastTransaction != null)
                    {
                        string lastSequenceNumber = lastTransaction;

                        // Pastikan lastSequenceNumber memiliki format yang benar
                        if (lastSequenceNumber.Length == 16 && lastSequenceNumber.StartsWith("GC/"))
                        {
                            // Extract the sequence part (XXXXX) from the last sequence number
                            string sequencePart = lastSequenceNumber.Substring(11, 5);

                            if (int.TryParse(sequencePart, out int lastSequence))
                            {
                                // Increment sequence number
                                newSequence = lastSequence + 1;
                            }
                            else
                            {
                                // Handle the case where the sequence part is not a valid number
                                throw new FormatException($"The sequence part '{sequencePart}' is not a valid number.");
                            }
                        }
                        else
                        {
                            // Handle the case where lastSequenceNumber does not match expected format
                            throw new FormatException($"The last sequence number '{lastSequenceNumber}' does not match the expected format 'GC/YYYY/MM/XXXXX'.");
                        }
                    }

                    // Generate the new sequence number with format GC/YYYY/MM/XXXXX
                    string sequenceNumber = $"GC/{year}/{month}/{newSequence:D5}";

                    result.Reference = sequenceNumber;
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanServiceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GeneralConsultanServiceDto> Handle(CancelGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Get the entity from the database, or you can attach it directly if the ID is known
                var entity = new GeneralConsultanService { Id = request.GeneralConsultanServiceDto.Id };

                // Attach the entity to the context
                _unitOfWork.Repository<GeneralConsultanService>().Attach(entity);

                // Only update the specific fields you need
                entity.Status = request.GeneralConsultanServiceDto.Status;

                // Mark specific properties as modified
                _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.Status));

                // Save changes
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Clear cache if needed
                _cache.Remove("GetGeneralConsultanServiceQuery_");

                return entity.Adapt<GeneralConsultanServiceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Update

        #region Delete

        public async Task<bool> Handle(DeleteGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanService>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanService>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Delete

        #region GET General Consultan Service Log

        public async Task<List<GeneralConsultationServiceLogDto>> Handle(GetGeneralConsultationServiceLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetGeneralConsultationServiceLogQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<GeneralConsultationServiceLog>? result))
                {
                    result = await _unitOfWork.Repository<GeneralConsultationServiceLog>().Entities
                        .Include(z => z.GeneralConsultanService)
                        .Include(z => z.UserBy)

                        //.ThenInclude(z => z.Gender)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<GeneralConsultationServiceLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET General Consultan Service Log

        #region CREATE General Consultan Service Log

        public async Task<GeneralConsultationServiceLogDto> Handle(CreateGeneralConsultationLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultationServiceLog>().AddAsync(request.GeneralConsultanlogDto.Adapt<GeneralConsultationServiceLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultationServiceLogQuery_");

                return result.Adapt<GeneralConsultationServiceLogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultationServiceLogDto>> Handle(CreateListGeneralConsultationLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultationServiceLog>().AddAsync(request.GeneralConsultanlogDto.Adapt<List<GeneralConsultationServiceLog>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultationServiceLogQuery_");

                return result.Adapt<List<GeneralConsultationServiceLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE General Consultan Service Log

        #region Update General Consultan Service Log

        public async Task<GeneralConsultationServiceLogDto> Handle(UpdateGeneralConsultationLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultationServiceLog>().UpdateAsync(request.GeneralConsultanlogDto.Adapt<GeneralConsultationServiceLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultationServiceLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultationServiceLogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Update General Consultan Service Log

        #region Delete General Consultan Service Log

        public async Task<bool> Handle(DeleteGeneralConsultationLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultationServiceLog>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultationServiceLog>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultationServiceLogQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GeneralConsultanServiceDto> Handle(UpdateFormGeneralConsultanServiceNewRequest request, CancellationToken cancellationToken)
        {
            if (request.GeneralConsultanServiceDto == null) return new();

            var entity = new GeneralConsultanService { Id = request.GeneralConsultanServiceDto.Id };
            _unitOfWork.Repository<GeneralConsultanService>().Attach(entity);

            if (request.Status == EnumStatusGeneralConsultantService.Planned)
            {
                UpdatePlannedFields(entity, request.GeneralConsultanServiceDto);
            }
            else if (request.Status == EnumStatusGeneralConsultantService.NurseStation)
            {
                UpdateNurseStationFields(entity, request.GeneralConsultanServiceDto);
            }
            else if (request.Status == EnumStatusGeneralConsultantService.Physician)
            {
                UpdatePhysicianFields(entity, request.GeneralConsultanServiceDto, request.IsReferTo);
            }

            if (request.UserDto is not null)
            {
                var usr = new User { Id = request.UserDto.Id };

                _unitOfWork.Repository<User>().Attach(usr);
                usr.IsWeatherPatientAllergyIds = request.UserDto.IsWeatherPatientAllergyIds;
                usr.IsPharmacologyPatientAllergyIds = request.UserDto.IsPharmacologyPatientAllergyIds;
                usr.IsFoodPatientAllergyIds = request.UserDto.IsFoodPatientAllergyIds;

                usr.WeatherPatientAllergyIds = request.UserDto.WeatherPatientAllergyIds;
                usr.PharmacologyPatientAllergyIds = request.UserDto.PharmacologyPatientAllergyIds;
                usr.FoodPatientAllergyIds = request.UserDto.FoodPatientAllergyIds;

                usr.IsFamilyMedicalHistory = request.UserDto.IsFamilyMedicalHistory;
                usr.IsMedicationHistory = request.UserDto.IsMedicationHistory;
                usr.FamilyMedicalHistory = request.UserDto.FamilyMedicalHistory;
                usr.FamilyMedicalHistoryOther = request.UserDto.FamilyMedicalHistoryOther;
                usr.MedicationHistory = request.UserDto.MedicationHistory;
                usr.PastMedicalHistory = request.UserDto.PastMedicalHistory;

                usr.CurrentMobile = request.UserDto.CurrentMobile;

                _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.IsWeatherPatientAllergyIds));
                _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.IsPharmacologyPatientAllergyIds));
                _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.IsFoodPatientAllergyIds));
                _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.WeatherPatientAllergyIds));
                _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.PharmacologyPatientAllergyIds));
                _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.FoodPatientAllergyIds));
                _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.IsFamilyMedicalHistory));
                _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.IsMedicationHistory));
                _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.FamilyMedicalHistory));
                _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.FamilyMedicalHistoryOther));
                _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.MedicationHistory));
                _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.PastMedicalHistory));
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Adapt<GeneralConsultanServiceDto>();
        }

        private void UpdatePlannedFields(GeneralConsultanService entity, GeneralConsultanServiceDto dto)
        {
            entity.Status = dto.Status;
            entity.PatientId = dto.PatientId;
            entity.TypeRegistration = dto.TypeRegistration;
            entity.IsAlertInformationSpecialCase = dto.IsAlertInformationSpecialCase;
            entity.ClassType = dto.ClassType;
            entity.ServiceId = dto.ServiceId;
            entity.InsurancePolicyId = dto.InsurancePolicyId;
            entity.PratitionerId = dto.PratitionerId;
            entity.Payment = dto.Payment;
            entity.RegistrationDate = dto.RegistrationDate;
            entity.PregnancyStatusA = dto.PregnancyStatusA;
            entity.PregnancyStatusP = dto.PregnancyStatusP;
            entity.PregnancyStatusG = dto.PregnancyStatusG;
            entity.HPHT = dto.HPHT;
            entity.HPL = dto.HPL;

            SetPropertiesModified(entity, nameof(entity.Status), nameof(entity.PatientId), nameof(entity.TypeRegistration),
                nameof(entity.IsAlertInformationSpecialCase),
                nameof(entity.PregnancyStatusA),
                nameof(entity.PregnancyStatusP),
                nameof(entity.PregnancyStatusG),
                nameof(entity.HPHT),
                nameof(entity.HPL),
                nameof(entity.ClassType), nameof(entity.ServiceId),
                nameof(entity.InsurancePolicyId), nameof(entity.PratitionerId), nameof(entity.Payment), nameof(entity.RegistrationDate));
        }

        private void UpdateNurseStationFields(GeneralConsultanService entity, GeneralConsultanServiceDto dto)
        {
            entity.InformationFrom = dto.InformationFrom;
            entity.ClinicVisitTypes = dto.ClinicVisitTypes;
            entity.AwarenessId = dto.AwarenessId;
            entity.Weight = dto.Weight;
            entity.Height = dto.Height;
            entity.RR = dto.RR;
            entity.SpO2 = dto.SpO2;
            entity.WaistCircumference = dto.WaistCircumference;
            entity.BMIIndex = dto.BMIIndex;
            entity.BMIIndexString = dto.BMIIndexString;
            entity.ScrinningTriageScale = dto.ScrinningTriageScale;
            entity.E = dto.E;
            entity.V = dto.V;
            entity.M = dto.M;
            entity.LILA = dto.LILA;
            entity.Temp = dto.Temp;
            entity.HR = dto.HR;
            entity.Systolic = dto.Systolic;
            entity.LocationId = dto.LocationId;
            entity.DiastolicBP = dto.DiastolicBP;
            entity.PainScale = dto.PainScale;
            entity.BMIState = dto.BMIState;
            entity.RiskOfFalling = dto.RiskOfFalling;
            entity.RiskOfFallingDetail = dto.RiskOfFallingDetail;

            SetPropertiesModified(entity, nameof(entity.InformationFrom), nameof(entity.AwarenessId), nameof(entity.LocationId), nameof(entity.Weight),
                nameof(entity.Height), nameof(entity.RR), nameof(entity.SpO2), nameof(entity.WaistCircumference),
                 nameof(entity.LILA),
                nameof(entity.BMIIndex), nameof(entity.BMIIndexString), nameof(entity.ScrinningTriageScale), nameof(entity.ClinicVisitTypes),
                nameof(entity.E), nameof(entity.V), nameof(entity.M), nameof(entity.Temp), nameof(entity.HR),
                nameof(entity.Systolic), nameof(entity.DiastolicBP), nameof(entity.PainScale), nameof(entity.BMIState),
                nameof(entity.RiskOfFalling), nameof(entity.RiskOfFallingDetail));
        }

        private void UpdatePhysicianFields(GeneralConsultanService entity, GeneralConsultanServiceDto dto, bool isReferTo)
        {
            if (isReferTo)
            {
                entity.PPKRujukanCode = dto.PPKRujukanCode;
                entity.PPKRujukanName = dto.PPKRujukanName;
                entity.ReferVerticalSpesialisParentSpesialisName = dto.ReferVerticalSpesialisParentSpesialisName;
                entity.ReferVerticalSpesialisParentSubSpesialisName = dto.ReferVerticalSpesialisParentSubSpesialisName;
                entity.ReferReason = dto.ReferReason;

                SetPropertiesModified(entity,
                    nameof(entity.PPKRujukanCode),
                    nameof(entity.PPKRujukanName),
                    nameof(entity.ReferVerticalSpesialisParentSpesialisName),
                    nameof(entity.ReferVerticalSpesialisParentSubSpesialisName),
                    nameof(entity.ReferReason));
            }
            else
            {
                entity.LocationId = dto.LocationId;
                entity.PratitionerId = dto.PratitionerId;
                entity.HomeStatus = dto.HomeStatus;
                entity.IsSickLeave = dto.IsSickLeave;
                entity.StartDateSickLeave = dto.StartDateSickLeave;
                entity.EndDateSickLeave = dto.EndDateSickLeave;
                entity.IsMaternityLeave = dto.IsMaternityLeave;
                entity.StartMaternityLeave = dto.StartMaternityLeave;
                entity.EndMaternityLeave = dto.EndMaternityLeave;

                UpdateNurseStationFields(entity, dto); // Including NurseStation fields

                SetPropertiesModified(entity, nameof(entity.PratitionerId), nameof(entity.HomeStatus), nameof(entity.IsSickLeave),
                    nameof(entity.LocationId),
                    nameof(entity.PPKRujukanCode),
                    nameof(entity.PPKRujukanName),
                    nameof(entity.ReferVerticalSpesialisParentSpesialisName),
                    nameof(entity.ReferVerticalSpesialisParentSubSpesialisName),
                    nameof(entity.ReferReason),
                    nameof(entity.StartDateSickLeave),
                    nameof(entity.EndDateSickLeave), nameof(entity.IsMaternityLeave),
                    nameof(entity.StartMaternityLeave), nameof(entity.EndMaternityLeave));
            }
        }

        private void SetPropertiesModified(GeneralConsultanService entity, params string[] propertyNames)
        {
            foreach (var property in propertyNames)
            {
                _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, property);
            }
        }

        #region Fixed

        private async Task<string> GenerateReferenceNumber()
        {
            var currentDate = DateTime.Now;
            string year = currentDate.ToString("yyyy");
            string month = currentDate.ToString("MM");

            var lastTransaction = await _unitOfWork.Repository<GeneralConsultanService>().Entities
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.CreatedDate.Value.Year == currentDate.Year && x.CreatedDate.Value.Month == currentDate.Month)
                .OrderByDescending(x => x.Id)
                .Select(x => x.Reference)
                .FirstOrDefaultAsync();

            int newSequence = 1;

            if (lastTransaction != null)
            {
                string lastSequenceNumber = lastTransaction;

                // Pastikan lastSequenceNumber memiliki format yang benar
                if (lastSequenceNumber.Length == 16 && lastSequenceNumber.StartsWith("GC/"))
                {
                    // Extract the sequence part (XXXXX) from the last sequence number
                    string sequencePart = lastSequenceNumber.Substring(11, 5);

                    if (int.TryParse(sequencePart, out int lastSequence))
                    {
                        // Increment sequence number
                        newSequence = lastSequence + 1;
                    }
                    else
                    {
                        // Handle the case where the sequence part is not a valid number
                        throw new FormatException($"The sequence part '{sequencePart}' is not a valid number.");
                    }
                }
                else
                {
                    // Handle the case where lastSequenceNumber does not match expected format
                    throw new FormatException($"The last sequence number '{lastSequenceNumber}' does not match the expected format 'GC/YYYY/MM/XXXXX'.");
                }
            }

            // Generate the new sequence number with format GC/YYYY/MM/XXXXX
            string sequenceNumber = $"GC/{year}/{month}/{newSequence:D5}";

            return sequenceNumber;
        }

        #endregion Fixed

        public async Task<GeneralConsultanServiceDto> Handle(CreateFormGeneralConsultanServiceNewRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.GeneralConsultanServiceDto is null)
                    return new();

                GeneralConsultanServiceDto? e = null;

                if (!request.IsFollowUpPatient)
                {
                    if (request.UserDto is not null)
                    {
                        var usr = new User { Id = request.UserDto.Id };

                        _unitOfWork.Repository<User>().Attach(usr);
                        usr.IsWeatherPatientAllergyIds = request.UserDto.IsWeatherPatientAllergyIds;
                        usr.IsPharmacologyPatientAllergyIds = request.UserDto.IsPharmacologyPatientAllergyIds;
                        usr.IsFoodPatientAllergyIds = request.UserDto.IsFoodPatientAllergyIds;

                        usr.WeatherPatientAllergyIds = request.UserDto.WeatherPatientAllergyIds;
                        usr.PharmacologyPatientAllergyIds = request.UserDto.PharmacologyPatientAllergyIds;
                        usr.FoodPatientAllergyIds = request.UserDto.FoodPatientAllergyIds;

                        usr.IsFamilyMedicalHistory = request.UserDto.IsFamilyMedicalHistory;
                        usr.IsMedicationHistory = request.UserDto.IsMedicationHistory;
                        usr.FamilyMedicalHistory = request.UserDto.FamilyMedicalHistory;
                        usr.FamilyMedicalHistoryOther = request.UserDto.FamilyMedicalHistoryOther;
                        usr.MedicationHistory = request.UserDto.MedicationHistory;
                        usr.PastMedicalHistory = request.UserDto.PastMedicalHistory;

                        usr.CurrentMobile = request.UserDto.CurrentMobile;

                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.IsWeatherPatientAllergyIds));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.IsPharmacologyPatientAllergyIds));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.IsFoodPatientAllergyIds));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.WeatherPatientAllergyIds));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.PharmacologyPatientAllergyIds));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.FoodPatientAllergyIds));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.IsFamilyMedicalHistory));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.IsMedicationHistory));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.FamilyMedicalHistory));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.FamilyMedicalHistoryOther));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.MedicationHistory));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.PastMedicalHistory));
                    }

                    var entity = new GeneralConsultanService
                    {
                        PatientId = request.GeneralConsultanServiceDto.PatientId,
                        TypeRegistration = request.GeneralConsultanServiceDto.TypeRegistration,
                        IsAlertInformationSpecialCase = request.GeneralConsultanServiceDto.IsAlertInformationSpecialCase,
                        ClassType = request.GeneralConsultanServiceDto.ClassType,
                        ServiceId = request.GeneralConsultanServiceDto.ServiceId,
                        PratitionerId = request.GeneralConsultanServiceDto.PratitionerId,
                        Payment = request.GeneralConsultanServiceDto.Payment,
                        InsurancePolicyId = request.GeneralConsultanServiceDto.InsurancePolicyId,
                        RegistrationDate = request.GeneralConsultanServiceDto.RegistrationDate,
                        Status = request.Status,
                        IsGC = request.GeneralConsultanServiceDto.IsGC,
                        IsMaternity = request.GeneralConsultanServiceDto.IsMaternity,
                        IsVaccination = request.GeneralConsultanServiceDto.IsVaccination,
                        IsMcu = request.GeneralConsultanServiceDto.IsMcu,
                        IsAccident = request.GeneralConsultanServiceDto.IsAccident,
                        PregnancyStatusA = request.GeneralConsultanServiceDto.PregnancyStatusA,
                        PregnancyStatusP = request.GeneralConsultanServiceDto.PregnancyStatusP,
                        PregnancyStatusG = request.GeneralConsultanServiceDto.PregnancyStatusG,
                        HPHT = request.GeneralConsultanServiceDto.HPHT,
                        HPL = request.GeneralConsultanServiceDto.HPL,
                        AppointmentDate = request.GeneralConsultanServiceDto.AppointmentDate,
                        Reference = await GenerateReferenceNumber(),
                        LocationId = request.GeneralConsultanServiceDto.LocationId
                    };

                    // Tambahkan entitas baru ke repository dan simpan ke database
                    await _unitOfWork.Repository<GeneralConsultanService>().AddAsync(entity);
                    await _unitOfWork.SaveChangesAsync(cancellationToken); // Pastikan entitas disimpan dan ID tersedia

                    // Pastikan ID sudah ada
                    if (entity.Id > 0) // atau menggunakan kondisi lain yang sesuai
                    {
                        e = entity.Adapt<GeneralConsultanServiceDto>();
                    }
                }
                else
                {
                    var entity = new GeneralConsultanService
                    {
                        PatientId = request.GeneralConsultanServiceDto.PatientId,
                        TypeRegistration = request.GeneralConsultanServiceDto.TypeRegistration,
                        IsAlertInformationSpecialCase = request.GeneralConsultanServiceDto.IsAlertInformationSpecialCase,
                        ClassType = request.GeneralConsultanServiceDto.ClassType,
                        ServiceId = request.GeneralConsultanServiceDto.ServiceId,
                        PratitionerId = request.GeneralConsultanServiceDto.PratitionerId,
                        Payment = request.GeneralConsultanServiceDto.Payment,
                        InsurancePolicyId = request.GeneralConsultanServiceDto.InsurancePolicyId,
                        Status = EnumStatusGeneralConsultantService.Planned,
                        IsGC = request.GeneralConsultanServiceDto.IsGC,
                        IsMaternity = request.GeneralConsultanServiceDto.IsMaternity,
                        IsVaccination = request.GeneralConsultanServiceDto.IsVaccination,
                        IsMcu = request.GeneralConsultanServiceDto.IsMcu,
                        IsAccident = request.GeneralConsultanServiceDto.IsAccident,
                        PregnancyStatusA = request.GeneralConsultanServiceDto.PregnancyStatusA,
                        PregnancyStatusP = request.GeneralConsultanServiceDto.PregnancyStatusP,
                        PregnancyStatusG = request.GeneralConsultanServiceDto.PregnancyStatusG,
                        HPHT = request.GeneralConsultanServiceDto.HPHT,
                        HPL = request.GeneralConsultanServiceDto.HPL,
                        AppointmentDate = request.GeneralConsultanServiceDto.AppointmentDate,
                        Reference = await GenerateReferenceNumber(),
                        LocationId = request.GeneralConsultanServiceDto.LocationId
                    };

                    // Tambahkan entitas baru ke repository dan simpan ke database
                    await _unitOfWork.Repository<GeneralConsultanService>().AddAsync(entity);
                    await _unitOfWork.SaveChangesAsync(cancellationToken); // Pastikan entitas disimpan dan ID tersedia

                    // Pastikan ID sudah ada
                    if (entity.Id > 0) // atau menggunakan kondisi lain yang sesuai
                    {
                        e = entity.Adapt<GeneralConsultanServiceDto>();
                    }
                }

                return e;
            }
            catch (Exception ex)
            {
                // Tangani kesalahan atau log sesuai kebutuhan
                throw;
            }
        }

        public async Task<GeneralConsultanServiceDto> Handle(UpdateConfirmFormGeneralConsultanServiceNewRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.GeneralConsultanServiceDto is null)
                    return new();

                GeneralConsultanService? entity = null;

                if (request.Status == EnumStatusGeneralConsultantService.Confirmed)
                {
                    entity = new GeneralConsultanService { Id = request.GeneralConsultanServiceDto.Id };

                    _unitOfWork.Repository<GeneralConsultanService>().Attach(entity);

                    entity.Status = request.GeneralConsultanServiceDto.Status;
                    entity.PatientId = request.GeneralConsultanServiceDto.PatientId;
                    entity.TypeRegistration = request.GeneralConsultanServiceDto.TypeRegistration;
                    entity.IsAlertInformationSpecialCase = request.GeneralConsultanServiceDto.IsAlertInformationSpecialCase;
                    entity.ClassType = request.GeneralConsultanServiceDto.ClassType;
                    entity.ServiceId = request.GeneralConsultanServiceDto.ServiceId;
                    entity.PratitionerId = request.GeneralConsultanServiceDto.PratitionerId;
                    entity.Payment = request.GeneralConsultanServiceDto.Payment;
                    entity.RegistrationDate = request.GeneralConsultanServiceDto.RegistrationDate;

                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.Status));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.PatientId));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.TypeRegistration));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.IsAlertInformationSpecialCase));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.ClassType));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.ServiceId));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.PratitionerId));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.Payment));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.RegistrationDate));
                }
                else if (request.Status == EnumStatusGeneralConsultantService.Waiting)
                {
                    entity = new GeneralConsultanService { Id = request.GeneralConsultanServiceDto.Id };

                    _unitOfWork.Repository<GeneralConsultanService>().Attach(entity);

                    entity.Status = request.GeneralConsultanServiceDto.Status;
                    entity.PratitionerId = request.GeneralConsultanServiceDto.PratitionerId;

                    entity.InformationFrom = request.GeneralConsultanServiceDto.InformationFrom;
                    entity.AwarenessId = request.GeneralConsultanServiceDto.AwarenessId;
                    entity.Weight = request.GeneralConsultanServiceDto.Weight;
                    entity.Height = request.GeneralConsultanServiceDto.Height;
                    entity.RR = request.GeneralConsultanServiceDto.RR;
                    entity.SpO2 = request.GeneralConsultanServiceDto.SpO2;
                    entity.WaistCircumference = request.GeneralConsultanServiceDto.WaistCircumference;
                    entity.BMIIndex = request.GeneralConsultanServiceDto.BMIIndex;
                    entity.BMIIndexString = request.GeneralConsultanServiceDto.BMIIndexString;
                    entity.ScrinningTriageScale = request.GeneralConsultanServiceDto.ScrinningTriageScale;
                    entity.E = request.GeneralConsultanServiceDto.E;
                    entity.V = request.GeneralConsultanServiceDto.V;
                    entity.M = request.GeneralConsultanServiceDto.M;
                    entity.Temp = request.GeneralConsultanServiceDto.Temp;
                    entity.HR = request.GeneralConsultanServiceDto.HR;
                    entity.Systolic = request.GeneralConsultanServiceDto.Systolic;
                    entity.DiastolicBP = request.GeneralConsultanServiceDto.DiastolicBP;
                    entity.PainScale = request.GeneralConsultanServiceDto.PainScale;
                    entity.BMIState = request.GeneralConsultanServiceDto.BMIState;
                    entity.RiskOfFalling = request.GeneralConsultanServiceDto.RiskOfFalling;
                    entity.RiskOfFallingDetail = request.GeneralConsultanServiceDto.RiskOfFallingDetail;

                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.Status));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.PratitionerId));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.InformationFrom));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.AwarenessId));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.Weight));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.Height));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.RR));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.SpO2));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.WaistCircumference));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.BMIIndex));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.BMIIndexString));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.ScrinningTriageScale));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.E));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.V));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.M));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.Temp));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.HR));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.Systolic));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.DiastolicBP));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.PainScale));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.BMIState));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.RiskOfFalling));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.RiskOfFallingDetail));
                }
                else if (request.Status == EnumStatusGeneralConsultantService.NurseStation)
                {
                    entity = new GeneralConsultanService { Id = request.GeneralConsultanServiceDto.Id };

                    _unitOfWork.Repository<GeneralConsultanService>().Attach(entity);

                    entity.Status = request.GeneralConsultanServiceDto.Status;

                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.Status));
                }
                else if (request.Status == EnumStatusGeneralConsultantService.Physician)
                {
                    entity = new GeneralConsultanService { Id = request.GeneralConsultanServiceDto.Id };

                    _unitOfWork.Repository<GeneralConsultanService>().Attach(entity);

                    entity.Status = request.GeneralConsultanServiceDto.Status;

                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.Status));
                }
                else if (request.Status == EnumStatusGeneralConsultantService.Finished)
                {
                    entity = new GeneralConsultanService { Id = request.GeneralConsultanServiceDto.Id };

                    _unitOfWork.Repository<GeneralConsultanService>().Attach(entity);

                    entity.Status = request.GeneralConsultanServiceDto.Status;
                    entity.PratitionerId = request.GeneralConsultanServiceDto.PratitionerId;
                    entity.HomeStatus = request.GeneralConsultanServiceDto.HomeStatus;
                    entity.IsSickLeave = request.GeneralConsultanServiceDto.IsSickLeave;
                    entity.StartDateSickLeave = request.GeneralConsultanServiceDto.StartDateSickLeave;
                    entity.EndDateSickLeave = request.GeneralConsultanServiceDto.EndDateSickLeave;
                    entity.IsMaternityLeave = request.GeneralConsultanServiceDto.IsMaternityLeave;
                    entity.StartMaternityLeave = request.GeneralConsultanServiceDto.StartMaternityLeave;
                    entity.EndMaternityLeave = request.GeneralConsultanServiceDto.EndMaternityLeave;

                    entity.InformationFrom = request.GeneralConsultanServiceDto.InformationFrom;
                    entity.AwarenessId = request.GeneralConsultanServiceDto.AwarenessId;
                    entity.Weight = request.GeneralConsultanServiceDto.Weight;
                    entity.Height = request.GeneralConsultanServiceDto.Height;
                    entity.RR = request.GeneralConsultanServiceDto.RR;
                    entity.SpO2 = request.GeneralConsultanServiceDto.SpO2;
                    entity.WaistCircumference = request.GeneralConsultanServiceDto.WaistCircumference;
                    entity.BMIIndex = request.GeneralConsultanServiceDto.BMIIndex;
                    entity.BMIIndexString = request.GeneralConsultanServiceDto.BMIIndexString;
                    entity.ScrinningTriageScale = request.GeneralConsultanServiceDto.ScrinningTriageScale;
                    entity.E = request.GeneralConsultanServiceDto.E;
                    entity.V = request.GeneralConsultanServiceDto.V;
                    entity.M = request.GeneralConsultanServiceDto.M;
                    entity.Temp = request.GeneralConsultanServiceDto.Temp;
                    entity.HR = request.GeneralConsultanServiceDto.HR;
                    entity.LILA = request.GeneralConsultanServiceDto.LILA;
                    entity.Systolic = request.GeneralConsultanServiceDto.Systolic;
                    entity.DiastolicBP = request.GeneralConsultanServiceDto.DiastolicBP;
                    entity.PainScale = request.GeneralConsultanServiceDto.PainScale;
                    entity.BMIState = request.GeneralConsultanServiceDto.BMIState;
                    entity.RiskOfFalling = request.GeneralConsultanServiceDto.RiskOfFalling;
                    entity.RiskOfFallingDetail = request.GeneralConsultanServiceDto.RiskOfFallingDetail;

                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.Status));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.PratitionerId));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.PratitionerId));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.HomeStatus));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.InformationFrom));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.AwarenessId));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.Weight));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.Height));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.RR));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.SpO2));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.WaistCircumference));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.BMIIndex));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.BMIIndexString));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.ScrinningTriageScale));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.E));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.V));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.M));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.Temp));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.HR));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.Systolic));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.DiastolicBP));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.PainScale));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.BMIState));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.RiskOfFalling));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.RiskOfFallingDetail));

                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.IsSickLeave));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.StartDateSickLeave));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.EndDateSickLeave));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.IsMaternityLeave));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.StartMaternityLeave));
                    _unitOfWork.Repository<GeneralConsultanService>().SetPropertyModified(entity, nameof(entity.EndMaternityLeave));
                }

                // Clear cache if needed
                _cache.Remove("GetGeneralConsultanServiceQuery_");

                if (entity != null)
                {
                    if (request.UserDto is not null)
                    {
                        var usr = new User { Id = request.UserDto.Id };

                        _unitOfWork.Repository<User>().Attach(usr);
                        usr.IsWeatherPatientAllergyIds = request.UserDto.IsWeatherPatientAllergyIds;
                        usr.IsPharmacologyPatientAllergyIds = request.UserDto.IsPharmacologyPatientAllergyIds;
                        usr.IsFoodPatientAllergyIds = request.UserDto.IsFoodPatientAllergyIds;

                        usr.WeatherPatientAllergyIds = request.UserDto.WeatherPatientAllergyIds;
                        usr.PharmacologyPatientAllergyIds = request.UserDto.PharmacologyPatientAllergyIds;
                        usr.FoodPatientAllergyIds = request.UserDto.FoodPatientAllergyIds;

                        usr.IsFamilyMedicalHistory = request.UserDto.IsFamilyMedicalHistory;
                        usr.IsMedicationHistory = request.UserDto.IsMedicationHistory;
                        usr.FamilyMedicalHistory = request.UserDto.FamilyMedicalHistory;
                        usr.FamilyMedicalHistoryOther = request.UserDto.FamilyMedicalHistoryOther;
                        usr.MedicationHistory = request.UserDto.MedicationHistory;
                        usr.PastMedicalHistory = request.UserDto.PastMedicalHistory;

                        usr.CurrentMobile = request.UserDto.CurrentMobile;

                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.IsWeatherPatientAllergyIds));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.IsPharmacologyPatientAllergyIds));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.IsFoodPatientAllergyIds));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.WeatherPatientAllergyIds));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.PharmacologyPatientAllergyIds));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.FoodPatientAllergyIds));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.IsFamilyMedicalHistory));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.IsMedicationHistory));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.FamilyMedicalHistory));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.FamilyMedicalHistoryOther));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.MedicationHistory));
                        _unitOfWork.Repository<User>().SetPropertyModified(usr, nameof(usr.PastMedicalHistory));
                    }

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    // Pastikan ID sudah ada
                    if (entity.Id > 0) // atau menggunakan kondisi lain yang sesuai
                    {
                        return entity.Adapt<GeneralConsultanServiceDto>();
                    }
                }

                return new();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Delete General Consultan Service Log
    }
}