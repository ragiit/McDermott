
using McDermott.Application.Dtos.Transaction;
using static McDermott.Application.Features.Queries.Transaction.GeneralConsultanServiceQueryHandler;

namespace McDermott.Application.Features.Commands.Transaction
{
    public class GeneralConsultanServiceCommand
    {
        #region Create
        #region Create
        internal class CreateGeneralConsultantClinicalAssesmentHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateGeneralConsultantClinicalAssesmentRequest, GeneralConsultantClinicalAssesmentDto>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<GeneralConsultantClinicalAssesmentDto> Handle(CreateGeneralConsultantClinicalAssesmentRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<GeneralConsultantClinicalAssesment>().AddAsync(request.GeneralConsultantClinicalAssesmentDto.Adapt<GeneralConsultantClinicalAssesment>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<GeneralConsultantClinicalAssesmentDto>();
            }
        }

        internal class CreateListGeneralConsultantClinicalAssesmentRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateListGeneralConsultantClinicalAssesmentRequest, List<GeneralConsultantClinicalAssesmentDto>>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<List<GeneralConsultantClinicalAssesmentDto>> Handle(CreateListGeneralConsultantClinicalAssesmentRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<GeneralConsultantClinicalAssesment>().AddAsync(request.GeneralConsultantClinicalAssesmentDtos.Adapt<List<GeneralConsultantClinicalAssesment>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<List<GeneralConsultantClinicalAssesmentDto>>();
            }
        }
        #endregion
        #endregion

        #region Get
        public class GetGeneralConsultanServiceQuery : IRequest<List<GeneralConsultanServiceDto>>;

        public class GetGeneralConsultanServiceByIdQuery(int id) : IRequest<GeneralConsultanServiceDto>
        {
            public int Id { get; set; } = id;
        }

        public class GetGeneralConsultantClinicalAssesmentQuery(Expression<Func<GeneralConsultantClinicalAssesment, bool>>? predicate = null) : IRequest<List<GeneralConsultantClinicalAssesmentDto>>
        {
            public Expression<Func<GeneralConsultantClinicalAssesment, bool>> Predicate { get; } = predicate;
        }
        #endregion


        public class CreateGeneralConsultanServiceRequest(GeneralConsultanServiceDto GeneralConsultanServiceDto) : IRequest<GeneralConsultanServiceDto>
        {
            public GeneralConsultanServiceDto GeneralConsultanServiceDto { get; set; } = GeneralConsultanServiceDto;
        }

        public class UpdateGeneralConsultanServiceRequest : IRequest<bool>
        {
            public GeneralConsultanServiceDto GeneralConsultanServiceDto { get; set; }

            public UpdateGeneralConsultanServiceRequest(GeneralConsultanServiceDto GeneralConsultanServiceDto)
            {
                this.GeneralConsultanServiceDto = GeneralConsultanServiceDto;
            }
        }

        public class DeleteGeneralConsultanServiceRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteGeneralConsultanServiceRequest(int id)
            {
                Id = id;
            }
        }
        public class DeleteListGeneralConsultanServiceRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListGeneralConsultanServiceRequest(List<int> id)
            {
                Id = id;
            }
        }


        #region Update
        public class UpdateGeneralConsultantClinicalAssesmentRequest(GeneralConsultantClinicalAssesmentDto GeneralConsultantClinicalAssesmentDto) : IRequest<bool>
        {
            public GeneralConsultantClinicalAssesmentDto GeneralConsultantClinicalAssesmentDto { get; set; } = GeneralConsultantClinicalAssesmentDto;
        }

        #endregion
    }
}
