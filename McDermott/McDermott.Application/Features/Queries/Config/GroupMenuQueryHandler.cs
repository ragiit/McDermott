using static McDermott.Application.Features.Commands.Config.GroupMenuCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class GroupMenuQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetGroupMenuQuery, List<GroupMenuDto>>,
        IRequestHandler<CreateGroupMenuRequest, GroupMenuDto>,
        IRequestHandler<CreateListGroupMenuRequest, List<GroupMenuDto>>,
        IRequestHandler<UpdateGroupMenuRequest, GroupMenuDto>,
        IRequestHandler<UpdateListGroupMenuRequest, List<GroupMenuDto>>,
        IRequestHandler<DeleteGroupMenuRequest, bool>
    {
        #region GET

        public async Task<List<GroupMenuDto>> Handle(GetGroupMenuQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetGroupMenuQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<GroupMenu>? result))
                {
                    result = await _unitOfWork.Repository<GroupMenu>().GetAsync(
                        null,
                        x => x
                        .Include(z => z.Group)
                        .Include(x => x.Menu),
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<GroupMenuDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<GroupMenuDto> Handle(CreateGroupMenuRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GroupMenu>().AddAsync(request.GroupMenuDto.Adapt<GroupMenu>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupMenuQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GroupMenuDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GroupMenuDto>> Handle(CreateListGroupMenuRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GroupMenu>().AddAsync(request.GroupMenuDtos.Adapt<List<GroupMenu>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupMenuQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GroupMenuDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<GroupMenuDto> Handle(UpdateGroupMenuRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GroupMenu>().UpdateAsync(request.GroupMenuDto.Adapt<GroupMenu>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupMenuQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GroupMenuDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GroupMenuDto>> Handle(UpdateListGroupMenuRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GroupMenu>().UpdateAsync(request.GroupMenuDtos.Adapt<List<GroupMenu>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupMenuQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GroupMenuDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteGroupMenuRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GroupMenu>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GroupMenu>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupMenuQuery_"); // Ganti dengan key yang sesuai

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