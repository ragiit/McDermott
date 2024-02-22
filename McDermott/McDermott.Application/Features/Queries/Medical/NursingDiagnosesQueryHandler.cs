namespace McDermott.Application.Features.Queries.Medical
{
    public class NursingDiagnosesQueryHandler
    {
        #region Get
        internal class GetNursingDiagnosesQueryHandler : IRequestHandler<GetNursingDiagnosesQuery, List<NursingDiagnosesDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetNursingDiagnosesQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<NursingDiagnosesDto>> Handle(GetNursingDiagnosesQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await _unitOfWork.Repository<NursingDiagnoses>().GetAsync(
                        query.Predicate, cancellationToken: cancellationToken);

                    return result.Adapt<List<NursingDiagnosesDto>>();
                }
                catch (Exception e)
                {
                    return [];
                }
            }
        }
        #endregion

        #region Create
        internal class CreateNursingDiagnosesHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateNursingDiagnosesRequest, NursingDiagnosesDto>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<NursingDiagnosesDto> Handle(CreateNursingDiagnosesRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<NursingDiagnoses>().AddAsync(request.NursingDiagnosesDto.Adapt<NursingDiagnoses>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<NursingDiagnosesDto>();
            }
        }
        internal class CreateListNursingDiagnosesRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateListNursingDiagnosesRequest, List<NursingDiagnosesDto>>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<List<NursingDiagnosesDto>> Handle(CreateListNursingDiagnosesRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<NursingDiagnoses>().AddAsync(request.NursingDiagnosesDtos.Adapt<List<NursingDiagnoses>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<List<NursingDiagnosesDto>>();
            }
        }
        #endregion

        #region Update
        internal class UpdateNursingDiagnosesHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateNursingDiagnosesRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<bool> Handle(UpdateNursingDiagnosesRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<NursingDiagnoses>().UpdateAsync(request.NursingDiagnosesDto.Adapt<NursingDiagnoses>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
        #endregion

        #region Delete
        internal class DeleteNursingDiagnosesHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteNursingDiagnosesRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<bool> Handle(DeleteNursingDiagnosesRequest request, CancellationToken cancellationToken)
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<NursingDiagnoses>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<NursingDiagnoses>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
        #endregion
    }
}
