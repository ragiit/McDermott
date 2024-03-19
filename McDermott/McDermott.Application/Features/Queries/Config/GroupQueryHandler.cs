

namespace McDermott.Application.Features.Queries.Config
{
    public class GroupQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetGroupQuery, List<GroupDto>>,
        IRequestHandler<CreateGroupRequest, GroupDto>,
        IRequestHandler<CreateListGroupRequest, List<GroupDto>>,
        IRequestHandler<UpdateGroupRequest, GroupDto>,
        IRequestHandler<UpdateListGroupRequest, List<GroupDto>>,
        IRequestHandler<DeleteGroupRequest, bool>
    {

        #region GET
        public async Task<List<GroupDto>> Handle(GetGroupQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetGroupQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Group>? result))
                {
                    result = await _unitOfWork.Repository<Group>().GetAsync(
                        null,
                        null,
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<GroupDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region CREATE
        public async Task<GroupDto> Handle(CreateGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Group>().AddAsync(request.GroupDto.Adapt<Group>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupQuery_"); // Ganti dengan key yang sesuai 

                return result.Adapt<GroupDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GroupDto>> Handle(CreateListGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Group>().AddAsync(request.GroupDtos.Adapt<List<Group>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GroupDto>>();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region UPDATE
        public async Task<GroupDto> Handle(UpdateGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Group>().UpdateAsync(request.GroupDto.Adapt<Group>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GroupDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GroupDto>> Handle(UpdateListGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Group>().UpdateAsync(request.GroupDtos.Adapt<List<Group>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GroupDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region DELETE
        public async Task<bool> Handle(DeleteGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Group>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Group>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupQuery_"); // Ganti dengan key yang sesuai

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