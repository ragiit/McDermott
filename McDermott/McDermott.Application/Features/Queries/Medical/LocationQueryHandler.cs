using static McDermott.Application.Features.Commands.Medical.LocationCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class LocationQueryHandler
    {
        internal class GetAllLocationQueryHandler : IRequestHandler<GetLocationQuery, List<LocationDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllLocationQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<LocationDto>> Handle(GetLocationQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Location>().Entities
                        .Select(Location => Location.Adapt<LocationDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetLocationByIdQueryHandler : IRequestHandler<GetLocationByIdQuery, LocationDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetLocationByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<LocationDto> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Location>().GetByIdAsync(request.Id);

                return result.Adapt<LocationDto>();
            }
        }

        internal class CreateLocationHandler : IRequestHandler<CreateLocationRequest, LocationDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateLocationHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<LocationDto> Handle(CreateLocationRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Location>().AddAsync(request.LocationDto.Adapt<Location>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<LocationDto>();
            }
        }

        internal class UpdateLocationHandler : IRequestHandler<UpdateLocationRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateLocationHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateLocationRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Location>().UpdateAsync(request.LocationDto.Adapt<Location>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteLocationHandler : IRequestHandler<DeleteLocationRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteLocationHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteLocationRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Location>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListLocationHandler : IRequestHandler<DeleteListLocationRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListLocationHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListLocationRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Location>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}