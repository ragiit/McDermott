namespace McDermott.Application.Features.Queries.Patient
{
    public class InsurancePolicyQueryHandler
    {
        #region Get

        internal class GetInsurancePolicyQueryHandler : IRequestHandler<GetInsurancePolicyQuery, List<InsurancePolicyDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetInsurancePolicyQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<InsurancePolicyDto>> Handle(GetInsurancePolicyQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await _unitOfWork.Repository<InsurancePolicy>().GetAsync(
                      query.Predicate,
                          x => x
                          .Include(z => z.Insurance)
                          .Include(z => z.User), cancellationToken);

                    return result.Adapt<List<InsurancePolicyDto>>();
                }
                catch (Exception)
                {
                    return [];
                }
            }
        }

        internal class GetInsurancePolicyCountQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetInsurancePolicyCountQuery, long>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<long> Handle(GetInsurancePolicyCountQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    return await _unitOfWork.Repository<InsurancePolicy>().GetCountAsync(
                      query.Predicate, cancellationToken);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        #endregion Get

        #region Create

        internal class CreateInsurancePolicyHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateInsurancePolicyRequest, InsurancePolicyDto>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<InsurancePolicyDto> Handle(CreateInsurancePolicyRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<InsurancePolicy>().AddAsync(request.InsurancePolicyDto.Adapt<InsurancePolicy>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<InsurancePolicyDto>();
            }
        }

        #endregion Create

        #region Update

        internal class UpdateInsurancePolicyHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateInsurancePolicyRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<bool> Handle(UpdateInsurancePolicyRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<InsurancePolicy>().UpdateAsync(request.InsurancePolicyDto.Adapt<InsurancePolicy>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        #endregion Update

        #region Delete

        internal class DeleteInsurancePolicyHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteInsurancePolicyRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<bool> Handle(DeleteInsurancePolicyRequest request, CancellationToken cancellationToken)
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<InsurancePolicy>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<InsurancePolicy>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        #endregion Delete
    }
}