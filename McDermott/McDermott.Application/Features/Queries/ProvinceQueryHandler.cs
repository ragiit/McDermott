using static McDermott.Application.Features.Commands.CountryCommand;
using static McDermott.Application.Features.Commands.ProvinceCommand;

namespace McDermott.Application.Features.Queries
{
    public class ProvinceQueryHandler
    {
        internal class GetAllProvinceQueryHandler : IRequestHandler<GetProvinceQuery, List<ProvinceDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllProvinceQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<ProvinceDto>> Handle(GetProvinceQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Province>().Entities
                        .Include(x => x.Country)
                        .Select(Province => Province.Adapt<ProvinceDto>())
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetProvinceByIdQueryHandler : IRequestHandler<GetProvinceByIdQuery, ProvinceDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetProvinceByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ProvinceDto> Handle(GetProvinceByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Province>().GetByIdAsync(request.Id);

                return result.Adapt<ProvinceDto>();
            }
        }

        internal class CreateProvinceHandler : IRequestHandler<CreateProvinceRequest, ProvinceDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateProvinceHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ProvinceDto> Handle(CreateProvinceRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Province>().AddAsync(request.ProvinceDto.Adapt<Province>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<ProvinceDto>();
            }
        }

        internal class UpdateProvinceHandler : IRequestHandler<UpdateProvinceRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateProvinceHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateProvinceRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Province>().UpdateAsync(request.ProvinceDto.Adapt<Province>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteProvinceHandler : IRequestHandler<DeleteProvinceRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteProvinceHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteProvinceRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Province>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
        internal class DeleteListProvinceHandler : IRequestHandler<DeleteListProvinceRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListProvinceHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListProvinceRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Province>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}