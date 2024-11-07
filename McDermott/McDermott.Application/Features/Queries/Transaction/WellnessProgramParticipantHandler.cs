using McDermott.Application.Features.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramParticipantCommand;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class WellnessProgramParticipantHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetWellnessProgramParticipantQuery, (List<WellnessProgramParticipantDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleWellnessProgramParticipantQuery, WellnessProgramParticipantDto>, IRequestHandler<ValidateWellnessProgramParticipant, bool>,
     IRequestHandler<CreateWellnessProgramParticipantRequest, WellnessProgramParticipantDto>,
     IRequestHandler<BulkValidateWellnessProgramParticipant, List<WellnessProgramParticipantDto>>,
     IRequestHandler<CreateListWellnessProgramParticipantRequest, List<WellnessProgramParticipantDto>>,
     IRequestHandler<UpdateWellnessProgramParticipantRequest, WellnessProgramParticipantDto>,
     IRequestHandler<UpdateListWellnessProgramParticipantRequest, List<WellnessProgramParticipantDto>>,
     IRequestHandler<DeleteWellnessProgramParticipantRequest, bool>
    {
        #region GET

        public async Task<List<WellnessProgramParticipantDto>> Handle(BulkValidateWellnessProgramParticipant request, CancellationToken cancellationToken)
        {
            var CountryDtos = request.WellnessProgramParticipantsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            //var CountryNames = CountryDtos.Select(x => x.Name).Distinct().ToList();
            //var Codes = CountryDtos.Select(x => x.Code).Distinct().ToList();

            //var existingCountrys = await _unitOfWork.Repository<Country>()
            //    .Entities
            //    .AsNoTracking()
            //    .Where(v => CountryNames.Contains(v.Name) && Codes.Contains(v.Code))
            //    .ToListAsync(cancellationToken);

            //return existingCountrys.Adapt<List<CountryDto>>();

            return [];
        }

        public async Task<bool> Handle(ValidateWellnessProgramParticipant request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<WellnessProgramParticipant>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<WellnessProgramParticipantDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetWellnessProgramParticipantQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<WellnessProgramParticipant>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<WellnessProgramParticipant>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<WellnessProgramParticipant>)query).ThenBy(additionalOrderBy.OrderBy);
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
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new WellnessProgramParticipant
                    {
                        Id = x.Id,
                        WellnessProgramId = x.WellnessProgramId,
                        PatientId = x.PatientId,
                        Patient = new User
                        {
                            Name = x.Patient == null ? "" : x.Patient.Name,
                            Email = x.Patient == null ? "" : x.Patient.Email,
                            Department = x.Patient != null && x.Patient.Department == null ? new Department() : new Department
                            {
                                Name = x.Patient != null && x.Patient.Department != null ? x.Patient.Department.Name : ""
                            }
                        },
                        Date = x.Date,
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<WellnessProgramParticipantDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<WellnessProgramParticipantDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<WellnessProgramParticipantDto> Handle(GetSingleWellnessProgramParticipantQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<WellnessProgramParticipant>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<WellnessProgramParticipant>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<WellnessProgramParticipant>)query).ThenBy(additionalOrderBy.OrderBy);
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
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new WellnessProgramParticipant
                    {
                        Id = x.Id,
                        WellnessProgramId = x.WellnessProgramId,
                        PatientId = x.PatientId,
                        Date = x.Date,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<WellnessProgramParticipantDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<WellnessProgramParticipantDto> Handle(CreateWellnessProgramParticipantRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgramParticipant>().AddAsync(request.WellnessProgramParticipantDto.Adapt<CreateUpdateWellnessProgramParticipantDto>().Adapt<WellnessProgramParticipant>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramParticipantQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<WellnessProgramParticipantDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<WellnessProgramParticipantDto>> Handle(CreateListWellnessProgramParticipantRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgramParticipant>().AddAsync(request.WellnessProgramParticipantDtos.Adapt<List<WellnessProgramParticipant>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramParticipantQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<WellnessProgramParticipantDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<WellnessProgramParticipantDto> Handle(UpdateWellnessProgramParticipantRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgramParticipant>().UpdateAsync(request.WellnessProgramParticipantDto.Adapt<WellnessProgramParticipantDto>().Adapt<WellnessProgramParticipant>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramParticipantQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<WellnessProgramParticipantDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<WellnessProgramParticipantDto>> Handle(UpdateListWellnessProgramParticipantRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgramParticipant>().UpdateAsync(request.WellnessProgramParticipantDtos.Adapt<List<WellnessProgramParticipant>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramParticipantQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<WellnessProgramParticipantDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteWellnessProgramParticipantRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<WellnessProgramParticipant>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<WellnessProgramParticipant>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramParticipantQuery_"); // Ganti dengan key yang sesuai

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