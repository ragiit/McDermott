using static McDermott.Application.Features.Commands.Medical.SpecialityCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class SpecialityQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetSpecialityQuery, (List<SpecialityDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateSpecialityRequest, SpecialityDto>,
        IRequestHandler<CreateListSpecialityRequest, List<SpecialityDto>>,
        IRequestHandler<UpdateSpecialityRequest, SpecialityDto>,
        IRequestHandler<UpdateListSpecialityRequest, List<SpecialityDto>>,
        IRequestHandler<DeleteSpecialityRequest, bool>
    {
        #region GET

        public async Task<(List<SpecialityDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetSpecialityQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Speciality>().Entities
                    .AsNoTracking()
                    .AsQueryable();

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

                return (paged.Adapt<List<SpecialityDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateSpecialityQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Speciality>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<SpecialityDto> Handle(CreateSpecialityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Speciality>().AddAsync(request.SpecialityDto.Adapt<Speciality>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSpecialityQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<SpecialityDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SpecialityDto>> Handle(CreateListSpecialityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Speciality>().AddAsync(request.SpecialityDtos.Adapt<List<Speciality>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSpecialityQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<SpecialityDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<SpecialityDto> Handle(UpdateSpecialityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Speciality>().UpdateAsync(request.SpecialityDto.Adapt<Speciality>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSpecialityQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<SpecialityDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SpecialityDto>> Handle(UpdateListSpecialityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Speciality>().UpdateAsync(request.SpecialityDtos.Adapt<List<Speciality>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSpecialityQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<SpecialityDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteSpecialityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Speciality>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Speciality>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSpecialityQuery_"); // Ganti dengan key yang sesuai

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