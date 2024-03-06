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

        #endregion Create

        #endregion Create

        #region Get

        public class GetGeneralConsultanServiceQuery : IRequest<List<GeneralConsultanServiceDto>>;

        public class GetGeneralConsultanServiceByIdQuery(long id) : IRequest<GeneralConsultanServiceDto>
        {
             public long Id { get; set; } = id;
        }

        public class GetGeneralConsultantClinicalAssesmentQuery(Expression<Func<GeneralConsultantClinicalAssesment, bool>>? predicate = null) : IRequest<List<GeneralConsultantClinicalAssesmentDto>>
        {
            public Expression<Func<GeneralConsultantClinicalAssesment, bool>> Predicate { get; } = predicate;
        }

        #endregion Get

        public class CreateGeneralConsultanServiceRequest(GeneralConsultanServiceDto GeneralConsultanServiceDto) : IRequest<GeneralConsultanServiceDto>
        {
            public GeneralConsultanServiceDto GeneralConsultanServiceDto { get; set; } = GeneralConsultanServiceDto;
        }

        public class UpdateGeneralConsultanServiceRequest(GeneralConsultanServiceDto GeneralConsultanServiceDto) : IRequest<bool>
        {
            public GeneralConsultanServiceDto GeneralConsultanServiceDto { get; set; } = GeneralConsultanServiceDto;
        }

        public class DeleteGeneralConsultanServiceRequest : IRequest<bool>
        {
             public long Id { get; set; }

            public DeleteGeneralConsultanServiceRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListGeneralConsultanServiceRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListGeneralConsultanServiceRequest(List<long> id)
            {
                Id = id;
            }
        }

        #region Update

        public class UpdateGeneralConsultantClinicalAssesmentRequest(GeneralConsultantClinicalAssesmentDto GeneralConsultantClinicalAssesmentDto) : IRequest<bool>
        {
            public GeneralConsultantClinicalAssesmentDto GeneralConsultantClinicalAssesmentDto { get; set; } = GeneralConsultantClinicalAssesmentDto;
        }

        #endregion Update
    }
}