using McDermott.Application.Features.Services;
using McDermott.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Transaction.GCReferToInternalCommand;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class GCReferToInternalQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
         IRequestHandler<GetGCReferToInternalQuery, (List<GCReferToInternalDto>, int pageIndex, int pageSize, int pageCount)>, //GCReferToInternal
        IRequestHandler<GetSingleGCReferToInternalQuery, GCReferToInternalDto>, IRequestHandler<ValidateGCReferToInternalQuery, bool>,
        IRequestHandler<BulkValidateGCReferToInternalQuery, List<GCReferToInternalDto>>,
        IRequestHandler<CreateGCReferToInternalRequest, GCReferToInternalDto>,
        IRequestHandler<CreateListGCReferToInternalRequest, List<GCReferToInternalDto>>,
        IRequestHandler<UpdateGCReferToInternalRequest, GCReferToInternalDto>,
        IRequestHandler<UpdateListGCReferToInternalRequest, List<GCReferToInternalDto>>,
        IRequestHandler<DeleteGCReferToInternalRequest, bool>
    {
        #region GET Goods Receipt

        public async Task<List<GCReferToInternalDto>> Handle(BulkValidateGCReferToInternalQuery request, CancellationToken cancellationToken)
        {
            var GCReferToInternalDtos = request.GCReferToInternalToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var GCReferToInternalNames = GCReferToInternalDtos.Select(x => x.TypeClaim).Distinct().ToList();

            var existingGCReferToInternals = await _unitOfWork.Repository<GCReferToInternal>()
                .Entities
                .AsNoTracking()
                .Where(v => GCReferToInternalNames.Contains(v.TypeClaim))
                .ToListAsync(cancellationToken);

            return existingGCReferToInternals.Adapt<List<GCReferToInternalDto>>();
        }

        public async Task<bool> Handle(ValidateGCReferToInternalQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<GCReferToInternal>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<GCReferToInternalDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGCReferToInternalQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GCReferToInternal>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<GCReferToInternal>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GCReferToInternal>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.TypeClaim, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.ExamFor, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GCReferToInternal
                    {
                        Id = x.Id,
                        TypeClaim = x.TypeClaim,
                        ExamFor = x.ExamFor,
                        CategoryRJMCINT = x.CategoryRJMCINT,
                        Hospital = x.Hospital,
                        Number = x.Number,
                        TempDiagnosis = x.TempDiagnosis,
                        TherapyProvide = x.TherapyProvide,
                        ReferTo = x.ReferTo,
                        DateRJMCINT = x.DateRJMCINT,
                        Specialist = x.Specialist,
                        GeneralConsultanServiceId =x.GeneralConsultanServiceId,
                        GeneralConsultanService = new GeneralConsultanService
                        {
                            PatientId =x.GeneralConsultanService.PatientId,
                            Patient= new User
                            {
                               Name = x.GeneralConsultanService.Patient == null ? string.Empty :x.GeneralConsultanService.Patient.Name
                            }
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

                    return (pagedItems.Adapt<List<GCReferToInternalDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<GCReferToInternalDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<GCReferToInternalDto> Handle(GetSingleGCReferToInternalQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GCReferToInternal>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<GCReferToInternal>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GCReferToInternal>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.TypeClaim, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.ExamFor, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GCReferToInternal
                    {
                        Id = x.Id,
                        TypeClaim=x.TypeClaim,
                        ExamFor=x.ExamFor,
                        CategoryRJMCINT=x.CategoryRJMCINT,
                        Hospital=x.Hospital,
                        Number=x.Number,
                        TempDiagnosis=x.TempDiagnosis,
                        TherapyProvide = x.TherapyProvide,
                        ReferTo=x.ReferTo,
                        DateRJMCINT=x.DateRJMCINT,
                        Specialist=x.Specialist,
                        GeneralConsultanServiceId = x.GeneralConsultanServiceId,
                        GeneralConsultanService = new GeneralConsultanService
                        {
                            PatientId = x.GeneralConsultanService.PatientId,
                            Patient = new User
                            {
                                Name = x.GeneralConsultanService.Patient == null ? string.Empty : x.GeneralConsultanService.Patient.Name
                            }
                        }


                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GCReferToInternalDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET Goods Receipt

        #region CREATE Goods Receipt

        public async Task<GCReferToInternalDto> Handle(CreateGCReferToInternalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GCReferToInternal>().AddAsync(request.GCReferToInternalDto.Adapt<GCReferToInternal>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGCReferToInternalQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GCReferToInternalDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GCReferToInternalDto>> Handle(CreateListGCReferToInternalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GCReferToInternal>().AddAsync(request.GCReferToInternalDtos.Adapt<List<GCReferToInternal>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGCReferToInternalQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GCReferToInternalDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Goods Receipt

        #region UPDATE Goods Receipt

        public async Task<GCReferToInternalDto> Handle(UpdateGCReferToInternalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GCReferToInternal>().UpdateAsync(request.GCReferToInternalDto.Adapt<GCReferToInternal>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGCReferToInternalQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GCReferToInternalDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GCReferToInternalDto>> Handle(UpdateListGCReferToInternalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GCReferToInternal>().UpdateAsync(request.GCReferToInternalDtos.Adapt<List<GCReferToInternal>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGCReferToInternalQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GCReferToInternalDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Goods Receipt

        #region DELETE Goods Receipt

        public async Task<bool> Handle(DeleteGCReferToInternalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GCReferToInternal>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GCReferToInternal>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGCReferToInternalQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE Goods Receipt



    }
}
