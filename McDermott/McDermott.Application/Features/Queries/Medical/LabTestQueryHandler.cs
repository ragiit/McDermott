using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Medical.LabTestCommand;

using static McDermott.Application.Features.Commands.Medical.LabTestCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class LabTestQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetLabTestQuery, (List<LabTestDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleLabTestQuery, LabTestDto>,
        IRequestHandler<ValidateLabTestQuery, bool>,
        IRequestHandler<BulkValidateLabTestQuery, List<LabTestDto>>,
        IRequestHandler<CreateLabTestRequest, LabTestDto>,
        IRequestHandler<CreateListLabTestRequest, List<LabTestDto>>,
        IRequestHandler<UpdateLabTestRequest, LabTestDto>,
        IRequestHandler<UpdateListLabTestRequest, List<LabTestDto>>,
        IRequestHandler<DeleteLabTestRequest, bool>
    {
        #region GET

        public async Task<List<LabTestDto>> Handle(BulkValidateLabTestQuery request, CancellationToken cancellationToken)
        {
            var LabTestDtos = request.LabTestsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var A = LabTestDtos.Select(x => x.Name).Distinct().ToList();
            var B = LabTestDtos.Select(x => x.Code).Distinct().ToList();
            var C = LabTestDtos.Select(x => x.ResultType).Distinct().ToList();
            var D = LabTestDtos.Select(x => x.SampleTypeId).Distinct().ToList();

            var existingLabTests = await _unitOfWork.Repository<LabTest>()
                .Entities
                .AsNoTracking()
                .Where(v => A.Contains(v.Name)
                            && B.Contains(v.Code)
                            && C.Contains(v.ResultType)
                            && D.Contains(v.SampleTypeId)
                            )
                .ToListAsync(cancellationToken);

            return existingLabTests.Adapt<List<LabTestDto>>();
        }

        public async Task<(List<LabTestDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetLabTestQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<LabTest>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<LabTest>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<LabTest>)query).ThenBy(additionalOrderBy.OrderBy);
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
                             EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Code, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.SampleType.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.ResultType, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new LabTest
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        ResultType = x.ResultType,
                        SampleTypeId = x.SampleTypeId,
                        SampleType = new SampleType
                        {
                            Name = x.SampleType.Name
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

                    return (pagedItems.Adapt<List<LabTestDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<LabTestDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<LabTestDto> Handle(GetSingleLabTestQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<LabTest>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<LabTest>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<LabTest>)query).ThenBy(additionalOrderBy.OrderBy);
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
                             EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                             EF.Functions.Like(v.Code, $"%{request.SearchTerm}%") ||
                             EF.Functions.Like(v.SampleType.Name, $"%{request.SearchTerm}%") ||
                             EF.Functions.Like(v.ResultType, $"%{request.SearchTerm}%")
                             );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new LabTest
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        ResultType = x.ResultType,
                        SampleTypeId = x.SampleTypeId,
                        SampleType = new SampleType
                        {
                            Name = x.SampleType.Name
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<LabTestDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<bool> Handle(ValidateLabTestQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<LabTest>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<LabTestDto> Handle(CreateLabTestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabTest>().AddAsync(request.LabTestDto.Adapt<LabTest>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LabTestDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LabTestDto>> Handle(CreateListLabTestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabTest>().AddAsync(request.LabTestDtos.Adapt<List<LabTest>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LabTestDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<LabTestDto> Handle(UpdateLabTestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabTest>().UpdateAsync(request.LabTestDto.Adapt<LabTest>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LabTestDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LabTestDto>> Handle(UpdateListLabTestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabTest>().UpdateAsync(request.LabTestDtos.Adapt<List<LabTest>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LabTestDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteLabTestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<LabTest>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<LabTest>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestQuery_"); // Ganti dengan key yang sesuai

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