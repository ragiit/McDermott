using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Config.GroupCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class GroupQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetGroupQuery, (List<GroupDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<ValidateGroupQuery, bool>,
        IRequestHandler<CreateGroupRequest, GroupDto>,
        IRequestHandler<CreateListGroupRequest, List<GroupDto>>,
        IRequestHandler<UpdateGroupRequest, GroupDto>,
        IRequestHandler<UpdateListGroupRequest, List<GroupDto>>,
        IRequestHandler<DeleteGroupRequest, bool>
    {
        #region GET

        public async Task<(List<GroupDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGroupQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Group>().Entities
                    .AsNoTracking()
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%"));
                }

                var pagedResult = query
                            .OrderBy(x => x.Name);

                var (totalCount, paged, totalPages) = await PaginateAsyncClass.PaginateAsync(request.PageSize, request.PageIndex, query, pagedResult, cancellationToken);

                return (paged.Adapt<List<GroupDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateGroupQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Group>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

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

        #endregion CREATE

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

        #endregion UPDATE

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

        #endregion DELETE
    }
}