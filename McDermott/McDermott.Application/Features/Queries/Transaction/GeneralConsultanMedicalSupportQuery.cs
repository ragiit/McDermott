

namespace McDermott.Application.Features.Queries.Transaction
{
    public class GeneralConsultanMedicalSupportQuery
    {
        #region Get
        internal class GetGeneralConsultanMedicalSupportQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetGeneralConsultanMedicalSupportQuery, List<GeneralConsultanMedicalSupportDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public async Task<List<GeneralConsultanMedicalSupportDto>> Handle(GetGeneralConsultanMedicalSupportQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().GetAsync(
                        query.Predicate,
                            x => x.Include(z => z.GeneralConsultanService),
                            cancellationToken);

                    return result.Adapt<List<GeneralConsultanMedicalSupportDto>>();
                }
                catch (Exception e)
                {
                    return [];
                }
            }
        }
        #endregion

        #region Create
        internal class CreateGeneralConsultanMedicalSupportHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateGeneralConsultanMedicalSupportRequest, GeneralConsultanMedicalSupportDto>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<GeneralConsultanMedicalSupportDto> Handle(CreateGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().AddAsync(request.GeneralConsultanMedicalSupportDto.Adapt<GeneralConsultanMedicalSupport>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<GeneralConsultanMedicalSupportDto>();
            }
        }

        internal class CreateListGeneralConsultanMedicalSupportRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateListGeneralConsultanMedicalSupportRequest, List<GeneralConsultanMedicalSupportDto>>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<List<GeneralConsultanMedicalSupportDto>> Handle(CreateListGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().AddAsync(request.GeneralConsultanMedicalSupportDtos.Adapt<List<GeneralConsultanMedicalSupport>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<List<GeneralConsultanMedicalSupportDto>>();
            }
        }
        #endregion

        #region Update
        internal class UpdateGeneralConsultanMedicalSupportHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateGeneralConsultanMedicalSupportRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<bool> Handle(UpdateGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().UpdateAsync(request.GeneralConsultanMedicalSupportDto.Adapt<GeneralConsultanMedicalSupport>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
        #endregion

        #region Delete
        internal class DeleteGeneralConsultanMedicalSupportHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteGeneralConsultanMedicalSupportRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<bool> Handle(DeleteGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
        #endregion
    }
}
