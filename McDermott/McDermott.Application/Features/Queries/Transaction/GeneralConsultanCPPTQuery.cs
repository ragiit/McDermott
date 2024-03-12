namespace McDermott.Application.Features.Queries.Transaction
{
    public class GeneralConsultanCPPTQuery
    {
        #region Get

        internal class GetGeneralConsultanCPPTQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetGeneralConsultanCPPTQuery, List<GeneralConsultanCPPTDto>>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<List<GeneralConsultanCPPTDto>> Handle(GetGeneralConsultanCPPTQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await _unitOfWork.Repository<GeneralConsultanCPPT>().GetAsync(
                        query.Predicate,
                            x => x.Include(z => z.GeneralConsultanService),
                            cancellationToken);

                    return result.Adapt<List<GeneralConsultanCPPTDto>>();
                }
                catch (Exception)
                {
                    return [];
                }
            }
        }

        #endregion Get



        #region Update

        internal class UpdateGeneralConsultanCPPTHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateGeneralConsultanCPPTRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<bool> Handle(UpdateGeneralConsultanCPPTRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<GeneralConsultanCPPT>().UpdateAsync(request.GeneralConsultanCPPTDto.Adapt<GeneralConsultanCPPT>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        #endregion Update

        #region Delete

        internal class DeleteGeneralConsultanCPPTHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteGeneralConsultanCPPTRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<bool> Handle(DeleteGeneralConsultanCPPTRequest request, CancellationToken cancellationToken)
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanCPPT>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanCPPT>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        #endregion Delete
    }
}