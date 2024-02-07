using McDermott.Application.Dtos.Config;
using static McDermott.Application.Features.Commands.Config.CountryCommand;
using static McDermott.Application.Features.Commands.Config.DistrictCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class DistrictQueryHandler
    {
        internal class GetAllDistrictQueryHandler : IRequestHandler<GetDistrictQuery, List<DistrictDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllDistrictQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<DistrictDto>> Handle(GetDistrictQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<District>().Entities
                        .Include(x => x.City)
                        .Include(x => x.Province)
                        .Select(District => District.Adapt<DistrictDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetDistrictByIdQueryHandler : IRequestHandler<GetDistrictByIdQuery, DistrictDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetDistrictByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<DistrictDto> Handle(GetDistrictByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<District>().GetByIdAsync(request.Id);

                return result.Adapt<DistrictDto>();
            }
        }

        internal class CreateDistrictHandler : IRequestHandler<CreateDistrictRequest, DistrictDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateDistrictHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<DistrictDto> Handle(CreateDistrictRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<District>().AddAsync(request.DistrictDto.Adapt<District>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<DistrictDto>();
            }
        }

        internal class UpdateDistrictHandler : IRequestHandler<UpdateDistrictRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateDistrictHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateDistrictRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<District>().UpdateAsync(request.DistrictDto.Adapt<District>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteDistrictHandler : IRequestHandler<DeleteDistrictRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteDistrictHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteDistrictRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<District>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListDistrictHandler : IRequestHandler<DeleteListDistrictRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListDistrictHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListDistrictRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<District>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}