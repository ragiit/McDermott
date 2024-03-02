namespace McDermott.Application.Features.Commands.Medical
{
    public class PractitionerCommand
    {
        #region Get

        //public class GetUserPractitionerQuery : IRequest<List<UserDto>>;
        public class GetUserPractitionerQuery(Expression<Func<User, bool>>? predicate = null) : IRequest<List<UserDto>>
        {
            public Expression<Func<User, bool>> Predicate { get; } = predicate;
        }

        #endregion Get
    }
}