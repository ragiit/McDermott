using McDermott.Application.Dtos.AwarenessEvent;
using McDermott.Application.Features.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.AwarenessEvent.ParticipanEduCommand;

namespace McDermott.Application.Features.Queries.AwarenessEvent
{
    public class ParticipanEduQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetAllParticipanEduQuery, List<ParticipanEduDto>>,//ParticipanEdu
        IRequestHandler<GetParticipanEduQuery, (List<ParticipanEduDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleParticipanEduQuery, ParticipanEduDto>, IRequestHandler<ValidateParticipanEduQuery, bool>,
        IRequestHandler<BulkValidateParticipanEduQuery, List<ParticipanEduDto>>,
        IRequestHandler<CreateParticipanEduRequest, ParticipanEduDto>,
        IRequestHandler<CreateListParticipanEduRequest, List<ParticipanEduDto>>,
        IRequestHandler<UpdateParticipanEduRequest, ParticipanEduDto>,
        IRequestHandler<UpdateListParticipanEduRequest, List<ParticipanEduDto>>,
        IRequestHandler<DeleteParticipanEduRequest, bool>
    {
        #region GET Education Program

        public async Task<List<ParticipanEduDto>> Handle(GetAllParticipanEduQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllParticipanEduQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<ParticipanEdu>? result))
                {
                    result = await _unitOfWork.Repository<ParticipanEdu>().Entities
                        .Include(x=>x.Patient)
                        .Include(x=>x.EducationProgram)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ParticipanEduDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ParticipanEduDto>> Handle(BulkValidateParticipanEduQuery request, CancellationToken cancellationToken)
        {
            var ParticipanEduDtos = request.ParticipanEduToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var ParticipanEduNames = ParticipanEduDtos.Select(x => x.Patient.Name).Distinct().ToList();
            var a = ParticipanEduDtos.Select(x => x.EducationProgram.EventName).Distinct().ToList();

            var existingParticipanEdus = await _unitOfWork.Repository<ParticipanEdu>()
                .Entities
                .AsNoTracking()
                .Where(v => ParticipanEduNames.Contains(v.EducationProgram.EventName))
                .ToListAsync(cancellationToken);

            return existingParticipanEdus.Adapt<List<ParticipanEduDto>>();
        }

        public async Task<bool> Handle(ValidateParticipanEduQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<ParticipanEdu>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<ParticipanEduDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetParticipanEduQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<ParticipanEdu>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<ParticipanEdu>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<ParticipanEdu>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.EducationProgram.EventName, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new ParticipanEdu
                    {
                        Id = x.Id,
                        PatientId = x.PatientId,
                        EducationProgramId = x.EducationProgramId,
                        CreatedDate= x.CreatedDate,
                        Patient = new User
                        {
                            Name = x.Patient == null ? string.Empty : x.Patient.Name,
                            Email = x.Patient == null ? string.Empty : x.Patient.Email,
                            DepartmentId = x.Patient.DepartmentId,
                            Department = new Department
                            {
                                Name = x.Patient.Department.Name == null ? string.Empty : x.Patient.Department.Name
                            }
                        },
                        EducationProgram = new EducationProgram
                        {
                            EventName = x.EducationProgram == null ? string.Empty : x.EducationProgram.EventName
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

                    return (pagedItems.Adapt<List<ParticipanEduDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<ParticipanEduDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<ParticipanEduDto> Handle(GetSingleParticipanEduQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<ParticipanEdu>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<ParticipanEdu>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<ParticipanEdu>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.EducationProgram.EventName, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new ParticipanEdu
                    {
                        Id = x.Id,
                        PatientId = x.PatientId,
                        EducationProgramId = x.EducationProgramId,
                        CreatedDate = x.CreatedDate,
                        Patient = new User
                        {
                            Name = x.Patient == null ? string.Empty : x.Patient.Name,
                            Email = x.Patient == null ? string.Empty : x.Patient.Email,
                            DepartmentId = x.Patient.DepartmentId,
                            Department = new Department
                            {
                                Name = x.Patient.Department.Name == null ? string.Empty : x.Patient.Department.Name
                            }
                        },
                        EducationProgram = new EducationProgram
                        {
                            EventName = x.EducationProgram == null ? string.Empty : x.EducationProgram.EventName
                        }

                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<ParticipanEduDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }



        #endregion GET Education Program

        #region CREATE Education Program

        public async Task<ParticipanEduDto> Handle(CreateParticipanEduRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ParticipanEdu>().AddAsync(request.ParticipanEduDto.Adapt<ParticipanEdu>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetParticipanEduQuery_"); // Ganti dengan key yang sesuai 

                return result.Adapt<ParticipanEduDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ParticipanEduDto>> Handle(CreateListParticipanEduRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ParticipanEdu>().AddAsync(request.ParticipanEduDtos.Adapt<List<ParticipanEdu>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetParticipanEduQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ParticipanEduDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Education Program

        #region UPDATE Education Program

        public async Task<ParticipanEduDto> Handle(UpdateParticipanEduRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ParticipanEdu>().UpdateAsync(request.ParticipanEduDto.Adapt<ParticipanEdu>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);


                _cache.Remove("GetParticipanEduQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ParticipanEduDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ParticipanEduDto>> Handle(UpdateListParticipanEduRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ParticipanEdu>().UpdateAsync(request.ParticipanEduDtos.Adapt<List<ParticipanEdu>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetParticipanEduQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ParticipanEduDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Education Program

        #region DELETE Education Program

        public async Task<bool> Handle(DeleteParticipanEduRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<ParticipanEdu>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<ParticipanEdu>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetParticipanEduQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE Education Program

    }
}
