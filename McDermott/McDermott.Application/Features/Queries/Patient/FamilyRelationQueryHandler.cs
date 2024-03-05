using static McDermott.Application.Features.Commands.Patient.FamilyRelationCommand;

namespace McDermott.Application.Features.Queries.Patient
{
    public class FamilyRelationQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetFamilyQuery, List<FamilyDto>>,
        IRequestHandler<CreateFamilyRequest, FamilyDto>,
        IRequestHandler<CreateListFamilyRequest, List<FamilyDto>>,
        IRequestHandler<UpdateFamilyRequest, FamilyDto>,
        IRequestHandler<UpdateListFamilyRequest, List<FamilyDto>>,
        IRequestHandler<DeleteFamilyRequest, bool>
    {
        #region GET
        public async Task<List<FamilyDto>> Handle(GetFamilyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetFamilyQuery_{request.Predicate?.ToString()}"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique
                if (!_cache.TryGetValue(cacheKey, out List<Family>? result))
                {
                    result = await _unitOfWork.Repository<Family>().GetAsync(
                        request.Predicate,
                        cancellationToken: cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<FamilyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region CREATE
        public async Task<FamilyDto> Handle(CreateFamilyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Family>().AddAsync(request.FamilyDto.Adapt<Family>());

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
        #endregion

        #region UPDATE
        public async Task<FamilyDto> Handle(UpdateFamilyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Family>().UpdateAsync(request.FamilyDto.Adapt<Family>());

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
        #endregion

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
        #endregion
    }
}