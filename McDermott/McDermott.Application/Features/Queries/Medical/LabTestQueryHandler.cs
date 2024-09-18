using static McDermott.Application.Features.Commands.Medical.LabTestCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class LabTestQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetLabTestQuery, (List<LabTestDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<ValidateLabTestQuery, bool>,
        IRequestHandler<CreateLabTestRequest, LabTestDto>,
        IRequestHandler<CreateListLabTestRequest, List<LabTestDto>>,
        IRequestHandler<UpdateLabTestRequest, LabTestDto>,
        IRequestHandler<UpdateListLabTestRequest, List<LabTestDto>>,
        IRequestHandler<DeleteLabTestRequest, bool>
    {
        #region GET

        public async Task<(List<LabTestDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetLabTestQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<LabTest>().Entities
                    .Include(x => x.SampleType)
                    .AsNoTracking()
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%"));
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var pagedResult = query
                            .OrderBy(x => x.Name);

                var skip = (request.PageIndex) * (request.PageSize == 0 ? totalCount : request.PageSize);

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<LabTestDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
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