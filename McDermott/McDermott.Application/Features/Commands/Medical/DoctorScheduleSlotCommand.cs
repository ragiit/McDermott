namespace McDermott.Application.Features.Commands.Medical
{
    public class DoctorScheduleSlotCommand
    {
        #region GET 

        public class GetDoctorScheduleSlotQuery(Expression<Func<DoctorScheduleSlot, bool>>? predicate = null, bool removeCache = false) : IRequest<List<DoctorScheduleSlotDto>>
        {
            public Expression<Func<DoctorScheduleSlot, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
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
