

namespace McDermott.Application.Features.Queries.Medical
{
    public class BuildingQueryHandler
    {
        internal class GetAllBuildingQueryHandler : IRequestHandler<GetBuildingQuery, List<BuildingDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllBuildingQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<BuildingDto>> Handle(GetBuildingQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Building>().Entities
                        .Include(x => x.HealthCenter)
                        .Select(Building => Building.Adapt<BuildingDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetBuildingLocationsByBuildingIdQuery : IRequestHandler<GetBuildingLocationByBuildingIdRequest, List<BuildingLocationDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetBuildingLocationsByBuildingIdQuery(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<BuildingLocationDto>> Handle(GetBuildingLocationByBuildingIdRequest request, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<BuildingLocation>().Entities
                     .Include(x => x.Location)
                     .Where(x => x.BuildingId == request.BuildingId)
                     .Select(x => x.Adapt<BuildingLocationDto>())
                     .AsNoTracking()
                     .ToListAsync(cancellationToken);
            }
        }

        internal class DeleteBuildingLocationByBuildingIdHandler : IRequestHandler<DeleteBuildingLocationByIdRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteBuildingLocationByBuildingIdHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteBuildingLocationByIdRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    await _unitOfWork.Repository<BuildingLocation>().DeleteAsync(request.Id);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return true;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        internal class CreateBuildingLocationHandler : IRequestHandler<CreateBuildingLocationRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateBuildingLocationHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(CreateBuildingLocationRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    foreach (var item in request.BuildingLocationDtos)
                    {
                        await _unitOfWork.Repository<BuildingLocation>().AddAsync(item.Adapt<BuildingLocation>());
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

        internal class GetBuildingByIdQueryHandler : IRequestHandler<GetBuildingByIdQuery, BuildingDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetBuildingByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<BuildingDto> Handle(GetBuildingByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Building>().GetByIdAsync(request.Id);

                return result.Adapt<BuildingDto>();
            }
        }

        internal class CreateBuildingHandler : IRequestHandler<CreateBuildingRequest, BuildingDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateBuildingHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<BuildingDto> Handle(CreateBuildingRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Building>().AddAsync(request.BuildingDto.Adapt<Building>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<BuildingDto>();
            }
        }

        internal class UpdateBuildingHandler : IRequestHandler<UpdateBuildingRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateBuildingHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateBuildingRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Building>().UpdateAsync(request.BuildingDto.Adapt<Building>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteBuildingHandler : IRequestHandler<DeleteBuildingRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteBuildingHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteBuildingRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    await _unitOfWork.Repository<Building>().DeleteAsync(request.Id);

                    var a = await _unitOfWork.Repository<BuildingLocation>().GetAllAsync();
                    a = a.Where(x => x.BuildingId == request.Id).ToList();

                    foreach (var item in a)
                    {
                        await _unitOfWork.Repository<BuildingLocation>().DeleteAsync(item.Id);
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

        internal class DeleteListBuildingHandler : IRequestHandler<DeleteListBuildingRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListBuildingHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListBuildingRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Building>().DeleteAsync(request.Id);

                foreach (var item in request.Id)
                {
                    var a = await _unitOfWork.Repository<BuildingLocation>().GetAllAsync();
                    a = a.Where(x => x.BuildingId == item).ToList();

                    foreach (var i in a)
                    {
                        await _unitOfWork.Repository<BuildingLocation>().DeleteAsync(i.Id);
                    }
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}