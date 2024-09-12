namespace McDermott.Application.Features.Commands.Medical
{
    public class DoctorScheduleSlotCommand
    {
        #region GET 

        public class GetDoctorScheduleSlotQuery(Expression<Func<DoctorScheduleSlot, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<DoctorScheduleSlotDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<DoctorScheduleSlot, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
        }

        public class ValidateDoctorScheduleSlotQuery(Expression<Func<DoctorScheduleSlot, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<DoctorScheduleSlot, bool>> Predicate { get; } = predicate!;
        }

        #endregion  

        #region CREATE

        public class CreateDoctorScheduleSlotRequest(DoctorScheduleSlotDto DoctorScheduleSlotDto) : IRequest<DoctorScheduleSlotDto>
        {
            public DoctorScheduleSlotDto DoctorScheduleSlotDto { get; set; } = DoctorScheduleSlotDto;
        }

        public class CreateListDoctorScheduleSlotRequest(List<DoctorScheduleSlotDto> DoctorScheduleSlotDtos) : IRequest<List<DoctorScheduleSlotDto>>
        {
            public List<DoctorScheduleSlotDto> DoctorScheduleSlotDtos { get; set; } = DoctorScheduleSlotDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateDoctorScheduleSlotRequest(DoctorScheduleSlotDto DoctorScheduleSlotDto) : IRequest<DoctorScheduleSlotDto>
        {
            public DoctorScheduleSlotDto DoctorScheduleSlotDto { get; set; } = DoctorScheduleSlotDto;
        }

        public class UpdateListDoctorScheduleSlotRequest(List<DoctorScheduleSlotDto> DoctorScheduleSlotDtos) : IRequest<List<DoctorScheduleSlotDto>>
        {
            public List<DoctorScheduleSlotDto> DoctorScheduleSlotDtos { get; set; } = DoctorScheduleSlotDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteDoctorScheduleSlotRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
