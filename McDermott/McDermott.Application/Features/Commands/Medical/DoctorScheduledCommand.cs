namespace McDermott.Application.Features.Commands.Medical
{
    public class DoctorScheduledCommand
    {
        #region GET

        public class GetDoctorScheduledQuery : IRequest<(List<DoctorScheduleDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<DoctorSchedule, object>>> Includes { get; set; }
            public Expression<Func<DoctorSchedule, bool>> Predicate { get; set; }
            public Expression<Func<DoctorSchedule, DoctorSchedule>> Select { get; set; }

            public List<(Expression<Func<DoctorSchedule, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetSingleDoctorScheduledQuery : IRequest<DoctorScheduleDto>
        {
            public List<Expression<Func<DoctorSchedule, object>>> Includes { get; set; }
            public Expression<Func<DoctorSchedule, bool>> Predicate { get; set; }
            public Expression<Func<DoctorSchedule, DoctorSchedule>> Select { get; set; }

            public List<(Expression<Func<DoctorSchedule, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateDoctorScheduledQuery(Expression<Func<DoctorSchedule, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<DoctorSchedule, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateDoctorScheduledRequest(DoctorScheduleDto DoctorScheduleDto) : IRequest<DoctorScheduleDto>
        {
            public DoctorScheduleDto DoctorScheduleDto { get; set; } = DoctorScheduleDto;
        }

        public class BulkValidateDoctorScheduledQuery(List<DoctorScheduleDto> DoctorSchedulesToValidate) : IRequest<List<DoctorScheduleDto>>
        {
            public List<DoctorScheduleDto> DoctorSchedulesToValidate { get; } = DoctorSchedulesToValidate;
        }

        public class CreateListDoctorScheduledRequest(List<DoctorScheduleDto> DoctorScheduleDtos) : IRequest<List<DoctorScheduleDto>>
        {
            public List<DoctorScheduleDto> DoctorScheduleDtos { get; set; } = DoctorScheduleDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateDoctorScheduledRequest(DoctorScheduleDto DoctorScheduleDto) : IRequest<DoctorScheduleDto>
        {
            public DoctorScheduleDto DoctorScheduleDto { get; set; } = DoctorScheduleDto;
        }

        public class UpdateListDoctorScheduledRequest(List<DoctorScheduleDto> DoctorScheduleDtos) : IRequest<List<DoctorScheduleDto>>
        {
            public List<DoctorScheduleDto> DoctorScheduleDtos { get; set; } = DoctorScheduleDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteDoctorScheduledRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        public class DeleteDoctorScheduledWithDetail(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}