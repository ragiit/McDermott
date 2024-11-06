using McDermott.Application.Dtos.AwarenessEvent;
using McDermott.Application.Features.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.AwarenessEvent.MaterialEduCommand;

namespace McDermott.Application.Features.Queries.AwarenessEvent
{
    public class MaterialEduQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetAllMaterialEduQuery, List<MaterialEduDto>>,//MaterialEdu
        IRequestHandler<GetMaterialEduQuery, (List<MaterialEduDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleMaterialEduQuery, MaterialEduDto>, IRequestHandler<ValidateMaterialEduQuery, bool>,
        IRequestHandler<CreateMaterialEduRequest, MaterialEduDto>,
        IRequestHandler<CreateListMaterialEduRequest, List<MaterialEduDto>>,
        IRequestHandler<UpdateMaterialEduRequest, MaterialEduDto>,
        IRequestHandler<UpdateListMaterialEduRequest, List<MaterialEduDto>>,
        IRequestHandler<DeleteMaterialEduRequest, bool>
    {
        #region GET Education Program

        public async Task<List<MaterialEduDto>> Handle(GetAllMaterialEduQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllMaterialEduQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<MaterialEdu>? result))
                {
                    result = await _unitOfWork.Repository<MaterialEdu>().Entities
                        .Include(x => x.EducationProgram)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<MaterialEduDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateMaterialEduQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<MaterialEdu>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<MaterialEduDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetMaterialEduQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<MaterialEdu>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<MaterialEdu>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<MaterialEdu>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.MaterialContent, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.EducationProgram.EventName, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new MaterialEdu
                    {
                        Id = x.Id,
                        MaterialContent = x.MaterialContent,
                        EducationProgramId = x.EducationProgramId,
                       Attendance =x.Attendance,
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

                    return (pagedItems.Adapt<List<MaterialEduDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<MaterialEduDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<MaterialEduDto> Handle(GetSingleMaterialEduQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<MaterialEdu>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<MaterialEdu>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<MaterialEdu>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.MaterialContent, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.EducationProgram.EventName, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new MaterialEdu
                    {
                        Id = x.Id,
                        MaterialContent = x.MaterialContent,
                        EducationProgramId = x.EducationProgramId,
                        Attendance = x.Attendance,
                        EducationProgram = new EducationProgram
                        {
                            EventName = x.EducationProgram == null ? string.Empty : x.EducationProgram.EventName
                        }

                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<MaterialEduDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }



        #endregion GET Education Program

        #region CREATE Education Program

        public async Task<MaterialEduDto> Handle(CreateMaterialEduRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaterialEdu>().AddAsync(request.MaterialEduDto.Adapt<MaterialEdu>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaterialEduQuery_"); // Ganti dengan key yang sesuai 

                return result.Adapt<MaterialEduDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MaterialEduDto>> Handle(CreateListMaterialEduRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaterialEdu>().AddAsync(request.MaterialEduDtos.Adapt<List<MaterialEdu>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaterialEduQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MaterialEduDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Education Program

        #region UPDATE Education Program

        public async Task<MaterialEduDto> Handle(UpdateMaterialEduRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaterialEdu>().UpdateAsync(request.MaterialEduDto.Adapt<MaterialEdu>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);


                _cache.Remove("GetMaterialEduQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MaterialEduDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MaterialEduDto>> Handle(UpdateListMaterialEduRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaterialEdu>().UpdateAsync(request.MaterialEduDtos.Adapt<List<MaterialEdu>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaterialEduQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MaterialEduDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Education Program

        #region DELETE Education Program

        public async Task<bool> Handle(DeleteMaterialEduRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<MaterialEdu>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<MaterialEdu>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaterialEduQuery_"); // Ganti dengan key yang sesuai

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
