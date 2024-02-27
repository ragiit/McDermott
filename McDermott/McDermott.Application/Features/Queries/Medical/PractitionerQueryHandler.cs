namespace McDermott.Application.Features.Queries.Medical
{
    public class PractitionerQueryHandler
    {
        #region Get
        //internal class GetAllUserEmployeeQueryHandler : IRequestHandler<GetUserPractitionerQuery, List<UserDto>>
        //{
        //    private readonly IUnitOfWork _unitOfWork;

        //    public GetAllUserEmployeeQueryHandler(IUnitOfWork unitOfWork)
        //    {
        //        _unitOfWork = unitOfWork;
        //    }

        //    public async Task<List<UserDto>> Handle(GetUserPractitionerQuery query, CancellationToken cancellationToken)
        //    {
        //        return await _unitOfWork.Repository<User>().Entities
        //                .Include(x => x.JobPosition)
        //                .Include(x => x.Department)
        //                .Where(x => x.IsDoctor == true)
        //                .Select(User => User.Adapt<UserDto>())
        //                .AsNoTracking()
        //                .ToListAsync(cancellationToken);
        //    }
        //}

        internal class GetUserPractitionerQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserPractitionerQuery, List<UserDto>>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<List<UserDto>> Handle(GetUserPractitionerQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await _unitOfWork.Repository<User>().GetAsync(
                        query.Predicate,
                            x => x
                           .Include(x => x.JobPosition)
                           .Include(x => x.Department), cancellationToken);

                    return result.Adapt<List<UserDto>>();
                }
                catch (Exception e)
                {
                    return [];
                }
            }
        }
        #endregion

    }
}
