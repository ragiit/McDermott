namespace McDermott.Application.Features.Commands.Employee
{
    public class SickLeaveCommand
    {
        #region GET

        public class GetSickLeaveQuery(Expression<Func<SickLeave, bool>>? predicate = null, bool removeCache = false) : IRequest<List<SickLeaveDto>>
        {
            public Expression<Func<SickLeave, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateSickLeaveRequest(SickLeaveDto SickLeaveDto) : IRequest<SickLeaveDto>
        {
            public SickLeaveDto SickLeaveDto { get; set; } = SickLeaveDto;
        }

        public class CreateListSickLeaveRequest(List<SickLeaveDto> SickLeaveDtos) : IRequest<List<SickLeaveDto>>
        {
            public List<SickLeaveDto> SickLeaveDtos { get; set; } = SickLeaveDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateSickLeaveRequest(SickLeaveDto SickLeaveDto) : IRequest<SickLeaveDto>
        {
            public SickLeaveDto SickLeaveDto { get; set; } = SickLeaveDto;
        }

        public class UpdateListSickLeaveRequest(List<SickLeaveDto> SickLeaveDtos) : IRequest<List<SickLeaveDto>>
        {
            public List<SickLeaveDto> SickLeaveDtos { get; set; } = SickLeaveDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteSickLeaveRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}