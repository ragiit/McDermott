namespace McDermott.Application.Features.Queries.Patient
{
    public class ClassTypeQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetClassTypeQuery, List<ClassTypeDto>>,
        IRequestHandler<CreateClassTypeRequest, ClassTypeDto>,
        IRequestHandler<CreateListClassTypeRequest, List<ClassTypeDto>>,
        IRequestHandler<UpdateClassTypeRequest, ClassTypeDto>,
        IRequestHandler<UpdateListClassTypeRequest, List<ClassTypeDto>>,
        IRequestHandler<DeleteClassTypeRequest, bool>
    {

        #region GET
        public async Task<List<ClassTypeDto>> Handle(GetClassTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetClassTypeQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique
                if (!_cache.TryGetValue(cacheKey, out List<ClassType>? result))
                {
                    result = await _unitOfWork.Repository<ClassType>().GetAsync(
                        request.Predicate,
                        cancellationToken: cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ClassTypeDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region CREATE
        public async Task<ClassTypeDto> Handle(CreateClassTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ClassType>().AddAsync(request.ClassTypeDto.Adapt<ClassType>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetClassTypeQuery_");

                return result.Adapt<ClassTypeDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ClassTypeDto>> Handle(CreateListClassTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ClassType>().AddAsync(request.ClassTypeDtos.Adapt<List<ClassType>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetClassTypeQuery_");

                return result.Adapt<List<ClassTypeDto>>();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region UPDATE
        public async Task<ClassTypeDto> Handle(UpdateClassTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ClassType>().UpdateAsync(request.ClassTypeDto.Adapt<ClassType>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetClassTypeQuery_");

                return result.Adapt<ClassTypeDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ClassTypeDto>> Handle(UpdateListClassTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ClassType>().UpdateAsync(request.ClassTypeDtos.Adapt<List<ClassType>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetClassTypeQuery_");

                return result.Adapt<List<ClassTypeDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region DELETE
        public async Task<bool> Handle(DeleteClassTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<ClassType>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<ClassType>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetClassTypeQuery_");

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
