namespace McDermott.Application.Features.Commands.Medical
{
    public class DoctorScheduleSlotCommand
    {
        #region GET

        public class GetSingleDoctorScheduleSlotQuery : IRequest<DoctorScheduleSlotDto>
        {
            public List<Expression<Func<DoctorScheduleSlot, object>>> Includes { get; set; }
            public Expression<Func<DoctorScheduleSlot, bool>> Predicate { get; set; }
            public Expression<Func<DoctorScheduleSlot, DoctorScheduleSlot>> Select { get; set; }

            public List<(Expression<Func<DoctorScheduleSlot, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetDoctorScheduleSlotQuery : IRequest<(List<DoctorScheduleSlotDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<DoctorScheduleSlot, object>>> Includes { get; set; }
            public Expression<Func<DoctorScheduleSlot, bool>> Predicate { get; set; }
            public Expression<Func<DoctorScheduleSlot, DoctorScheduleSlot>> Select { get; set; }

            public List<(Expression<Func<DoctorScheduleSlot, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateDoctorScheduleSlotQuery(Expression<Func<DoctorScheduleSlot, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<DoctorScheduleSlot, bool>> Predicate { get; } = predicate!;
        }

        public class BulkValidateDoctorScheduleSlotQuery(List<DoctorScheduleSlotDto> DoctorScheduleSlotsToValidate) : IRequest<List<DoctorScheduleSlotDto>>
        {
            public List<DoctorScheduleSlotDto> DoctorScheduleSlotsToValidate { get; } = DoctorScheduleSlotsToValidate;
        }

        #endregion GET

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