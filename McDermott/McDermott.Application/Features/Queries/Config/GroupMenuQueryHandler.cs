using static McDermott.Application.Features.Commands.Config.GroupMenuCommand;

using static McDermott.Application.Features.Commands.Config.GroupMenuCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class GroupMenuQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :

        IRequestHandler<GetGroupMenuQuery, (List<GroupMenuDto>, int pageIndex, int pageSize, int pageCount)>,
         IRequestHandler<ValidateGroupMenuQuery, bool>,
        IRequestHandler<CreateGroupMenuRequest, GroupMenuDto>,
        IRequestHandler<CreateListGroupMenuRequest, List<GroupMenuDto>>,
        IRequestHandler<UpdateGroupMenuRequest, GroupMenuDto>,
        IRequestHandler<UpdateListGroupMenuRequest, List<GroupMenuDto>>,
        IRequestHandler<DeleteGroupMenuRequest, bool>
    {
        #region GET

        public async Task<bool> Handle(ValidateGroupMenuQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<GroupMenu>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<GroupMenuDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGroupMenuQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GroupMenu>().Entities
                    .AsNoTracking()
                    .Include(v => v.Group)
                    .Include(v => v.Menu)
                    .ThenInclude(x => x.Parent)
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Group.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Menu.Name, $"%{request.SearchTerm}%"));
                }

                //var pagedResult = query
                //            .OrderBy(x => x.Group.Name);

                var totalCount = await query.CountAsync(cancellationToken);
                var skip = (request.PageIndex) * (request.PageSize == 0 ? totalCount : request.PageSize);

                var paged = query
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<GroupMenuDto>>(), request.PageIndex, request.PageSize, totalPages);
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