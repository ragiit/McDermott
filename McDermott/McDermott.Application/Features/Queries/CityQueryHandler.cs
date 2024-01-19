using static McDermott.Application.Features.Commands.CityCommand;

namespace McDermott.Application.Features.Queries
{
    public class CityQueryHandler
    {
        internal class GetAllCityQueryHandler : IRequestHandler<GetCityQuery, List<CityDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllCityQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<CityDto>> Handle(GetCityQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<City>().Entities
                        .Include(x => x.Province)
                        .Select(City => City.Adapt<CityDto>())
                       .ToListAsync(cancellationToken);
            }
        }

        internal class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, CityDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetCityByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<CityDto> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<City>().GetByIdAsync(request.Id);

                return result.Adapt<CityDto>();
            }
        }

        internal class CreateCityHandler : IRequestHandler<CreateCityRequest, CityDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateCityHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<CityDto> Handle(CreateCityRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<City>().AddAsync(request.CityDto.Adapt<City>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<CityDto>();
            }
        }

        internal class UpdateCityHandler : IRequestHandler<UpdateCityRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateCityHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateCityRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<City>().UpdateAsync(request.CityDto.Adapt<City>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteCityHandler : IRequestHandler<DeleteCityRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteCityHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteCityRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<City>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}