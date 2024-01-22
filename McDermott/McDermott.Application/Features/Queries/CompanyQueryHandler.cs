using static McDermott.Application.Features.Commands.CompanyCommand;
using static McDermott.Application.Features.Commands.CountryCommand;

namespace McDermott.Application.Features.Queries
{
    public class CompanyQueryHandler
    {
        internal class GetAllCompanyQueryHandler : IRequestHandler<GetCompanyQuery, List<CompanyDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllCompanyQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<CompanyDto>> Handle(GetCompanyQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Company>().Entities
                        .Include(x => x.Country)
                        .Include(x => x.Province)
                        .Include(x => x.City)
                        // .Include(x=>x.Currency)
                        .Select(Company => Company.Adapt<CompanyDto>())
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetCompanyByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Company>().GetByIdAsync(request.Id);

                return result.Adapt<CompanyDto>();
            }
        }

        internal class CreateCompanyHandler : IRequestHandler<CreateCompanyRequest, CompanyDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateCompanyHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<CompanyDto> Handle(CreateCompanyRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Company>().AddAsync(request.CompanyDto.Adapt<Company>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<CompanyDto>();
            }
        }

        internal class UpdateCompanyHandler : IRequestHandler<UpdateCompanyRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateCompanyHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateCompanyRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Company>().UpdateAsync(request.CompanyDto.Adapt<Company>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteCompanyHandler : IRequestHandler<DeleteCompanyRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteCompanyHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteCompanyRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Company>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListCompanyHandler : IRequestHandler<DeleteListCompanyRequest, bool>
        {
           private readonly IUnitOfWork _unitOfWork;

           public DeleteListCompanyHandler(IUnitOfWork unitOfWork)
           {
               _unitOfWork = unitOfWork;
           }

           public async Task<bool> Handle(DeleteListCompanyRequest request, CancellationToken cancellationToken)
           {
               await _unitOfWork.Repository<Company>().DeleteAsync(request.Id);
               await _unitOfWork.SaveChangesAsync(cancellationToken);

               return true;
           }
        }
    }
}