using MapsterMapper;
using static McDermott.Application.Features.Commands.CountryCommand;
using static McDermott.Application.Features.Commands.GroupCommand;

namespace McDermott.Application.Features.Queries
{
    public class GroupQueryHandler
    {
        internal class GetAllGroupQueryHandler : IRequestHandler<GetGroupQuery, List<GroupDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllGroupQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<GroupDto>> Handle(GetGroupQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Group>().Entities
                        .Select(Group => Group.Adapt<GroupDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetGroupMenusByGroupIdQuery : IRequestHandler<GetGroupMenuByGroupIdRequest, List<GroupMenuDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetGroupMenusByGroupIdQuery(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<GroupMenuDto>> Handle(GetGroupMenuByGroupIdRequest request, CancellationToken cancellationToken)
            {
                var a = await _unitOfWork.Repository<GroupMenu>().Entities
                     .Include(x => x.Menu)
                     .Where(x => x.GroupId == request.GroupId)
                     .Select(x => x.Adapt<GroupMenuDto>())
                     .ToListAsync(cancellationToken);

                return a;
            }
        }

        internal class GetGroupByNameQueryHandler : IRequestHandler<GetGroupByNameQuery, GroupDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetGroupByNameQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<GroupDto> Handle(GetGroupByNameQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Group>().Entities.Where(x => x.Name == request.Name).AsNoTracking().FirstOrDefaultAsync();

                return result.Adapt<GroupDto>();
            }
        }

        internal class UpdateGroupMenuHandler : IRequestHandler<UpdateGroupMenuRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateGroupMenuHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateGroupMenuRequest request, CancellationToken cancellationToken)
            {
                foreach (var item in request._ids)
                {
                    await _unitOfWork.Repository<GroupMenu>().DeleteAsync(item);
                }

                foreach (var item in request.GroupMenuDto)
                {
                    var a = item.Adapt<GroupMenu>();
                    await _unitOfWork.Repository<GroupMenu>().AddAsync(a);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class CreateGroupMenuHandler : IRequestHandler<CreateGroupMenuRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateGroupMenuHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(CreateGroupMenuRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    foreach (var item in request.GroupMenuDto)
                    {
                        var a = item.Adapt<GroupMenu>();
                        if (a.MenuId == 0) continue; // kalo menunya itu "All"

                        a.Menu = null;
                        a.Id = 0;

                        await _unitOfWork.Repository<GroupMenu>().AddAsync(a);
                    }

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        internal class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, GroupDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetGroupByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<GroupDto> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Group>().GetByIdAsync(request.Id);

                return result.Adapt<GroupDto>();
            }
        }

        internal class CreateGroupHandler : IRequestHandler<CreateGroupRequest, GroupDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateGroupHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<GroupDto> Handle(CreateGroupRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Group>().AddAsync(request.GroupDto.Adapt<Group>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<GroupDto>();
            }
        }

        internal class UpdateGroupHandler : IRequestHandler<UpdateGroupRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateGroupHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateGroupRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Group>().UpdateAsync(request.GroupDto.Adapt<Group>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteGroupHandler : IRequestHandler<DeleteGroupRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteGroupHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteGroupRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    await _unitOfWork.Repository<Group>().DeleteAsync(request.Id);

                    var groupMenus = await _unitOfWork.Repository<GroupMenu>().GetAllAsync();
                    groupMenus = groupMenus.Where(x => x.GroupId == request.Id).ToList();

                    foreach (var item in groupMenus)
                    {
                        await _unitOfWork.Repository<GroupMenu>().DeleteAsync(item.Id);
                    }

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return true;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        internal class DeleteGroupMenuByGroupIdHandler : IRequestHandler<DeleteGroupMenuByIdRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteGroupMenuByGroupIdHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteGroupMenuByIdRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    await _unitOfWork.Repository<GroupMenu>().DeleteAsync(request.Id);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return true;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        internal class DeleteListGroupMenuHandler : IRequestHandler<DeleteListGroupMenuRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListGroupMenuHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListGroupMenuRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    await _unitOfWork.Repository<Group>().DeleteAsync(request.Id);

                    foreach (var item in request.Id)
                    {
                        var groupMenus = await _unitOfWork.Repository<GroupMenu>().GetAllAsync();
                        groupMenus = groupMenus.Where(x => x.GroupId == item).ToList();

                        foreach (var i in groupMenus)
                        {
                            await _unitOfWork.Repository<GroupMenu>().DeleteAsync(i.Id);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return true;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
    }
}