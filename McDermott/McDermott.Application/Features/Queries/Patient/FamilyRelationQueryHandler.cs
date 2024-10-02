using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Patient.FamilyRelationCommand;

namespace McDermott.Application.Features.Queries.Patient
{
    public class FamilyRelationQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
       IRequestHandler<GetFamilyQuery, (List<FamilyDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<ValidateFamilyQuery, bool>,
        IRequestHandler<BulkValidateFamilyQuery, List<FamilyDto>>,
        IRequestHandler<CreateFamilyRequest, FamilyDto>,
        IRequestHandler<CreateListFamilyRequest, List<FamilyDto>>,
        IRequestHandler<UpdateFamilyRequest, FamilyDto>,
        IRequestHandler<UpdateListFamilyRequest, List<FamilyDto>>,
        IRequestHandler<DeleteFamilyRequest, bool>
    {
        #region GET

        public async Task<List<FamilyDto>> Handle(BulkValidateFamilyQuery request, CancellationToken cancellationToken)
        {
            var FamilyDtos = request.FamilysToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var FamilyNames = FamilyDtos.Select(x => x.Name).Distinct().ToList();
            var a = FamilyDtos.Select(x => x.InverseRelationId).Distinct().ToList();

            var existingFamilys = await _unitOfWork.Repository<Family>()
                .Entities
                .AsNoTracking()
                .Where(v => FamilyNames.Contains(v.Name)
                            && a.Contains(v.InverseRelationId))
                .ToListAsync(cancellationToken);

            return existingFamilys.Adapt<List<FamilyDto>>();
        }

        public async Task<(List<FamilyDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetFamilyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Family>().Entities.AsNoTracking();

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
                        EF.Functions.Like(v.InverseRelation.Name, $"%{request.SearchTerm}%"));
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

                return (pagedItems.Adapt<List<FamilyDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateFamilyQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Family>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<FamilyDto> Handle(CreateFamilyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Family>().AddAsync(request.FamilyDto.Adapt<CreateUpdateFamilyDto>().Adapt<Family>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetFamilyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<FamilyDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<FamilyDto>> Handle(CreateListFamilyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Family>().AddAsync(request.FamilyDtos.Adapt<List<Family>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetFamilyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<FamilyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<FamilyDto> Handle(UpdateFamilyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Family>().UpdateAsync(request.FamilyDto.Adapt<CreateUpdateFamilyDto>().Adapt<Family>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetFamilyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<FamilyDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<FamilyDto>> Handle(UpdateListFamilyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Family>().UpdateAsync(request.FamilyDtos.Adapt<List<Family>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetFamilyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<FamilyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteFamilyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Family>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Family>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetFamilyQuery_"); // Ganti dengan key yang sesuai

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