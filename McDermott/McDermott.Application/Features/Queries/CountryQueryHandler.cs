using static McDermott.Application.Features.Commands.CountryCommand;

namespace McDermott.Application.Features.Queries
{
    public class CountryQueryHandler
    {
        internal class GetAllCountryQueryHandler : IRequestHandler<GetCountryQuery, List<CountryDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllCountryQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<CountryDto>> Handle(GetCountryQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Country>().Entities
                        .Select(country => country.Adapt<CountryDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, CountryDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetCountryByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<CountryDto> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Country>().GetByIdAsync(request.Id);

                return result.Adapt<CountryDto>();
            }
        }

        internal class CreateCountryHandler : IRequestHandler<CreateCountryRequest, CountryDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateCountryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<CountryDto> Handle(CreateCountryRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Country>().AddAsync(request.CountryDto.Adapt<Country>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<CountryDto>();
            }
        }

        internal class UpdateCountryHandler : IRequestHandler<UpdateCountryRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateCountryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateCountryRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Country>().UpdateAsync(request.CountryDto.Adapt<Country>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteCountryHandler : IRequestHandler<DeleteCountryRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteCountryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteCountryRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Country>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListCountryHandler : IRequestHandler<DeleteListCountryRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListCountryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListCountryRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Country>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}