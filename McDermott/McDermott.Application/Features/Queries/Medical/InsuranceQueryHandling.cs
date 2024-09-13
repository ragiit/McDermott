using static McDermott.Application.Features.Commands.Medical.InsuranceCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class InsuranceQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetInsuranceQuery, (List<InsuranceDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateInsuranceRequest, InsuranceDto>,
        IRequestHandler<CreateListInsuranceRequest, List<InsuranceDto>>,
        IRequestHandler<UpdateInsuranceRequest, InsuranceDto>,
        IRequestHandler<UpdateListInsuranceRequest, List<InsuranceDto>>,
        IRequestHandler<DeleteInsuranceRequest, bool>
    {
        #region GET

        public async Task<(List<InsuranceDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetInsuranceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Insurance>().Entities
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Type, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.AdminFee.ToString(), $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Presentase.ToString(), $"%{request.SearchTerm}%"));
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var pagedResult = query
                            .OrderBy(x => x.Name);

                var skip = (request.PageIndex) * (request.PageSize == 0 ? totalCount : request.PageSize);

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<InsuranceDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateInsuranceQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Insurance>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<InsuranceDto> Handle(CreateInsuranceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Insurance>().AddAsync(request.InsuranceDto.Adapt<Insurance>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsuranceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<InsuranceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InsuranceDto>> Handle(CreateListInsuranceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Insurance>().AddAsync(request.InsuranceDtos.Adapt<List<Insurance>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsuranceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<InsuranceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<InsuranceDto> Handle(UpdateInsuranceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Insurance>().UpdateAsync(request.InsuranceDto.Adapt<Insurance>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsuranceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<InsuranceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InsuranceDto>> Handle(UpdateListInsuranceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Insurance>().UpdateAsync(request.InsuranceDtos.Adapt<List<Insurance>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsuranceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<InsuranceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteInsuranceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Insurance>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Insurance>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsuranceQuery_"); // Ganti dengan key yang sesuai

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