using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Transaction
{
    public class WellnessProgramAttendanceCommand
    {
        #region GET

        public class GetSingleWellnessProgramAttendanceQuery : IRequest<WellnessProgramAttendanceDto>
        {
            public List<Expression<Func<WellnessProgramAttendance, object>>> Includes { get; set; }
            public Expression<Func<WellnessProgramAttendance, bool>> Predicate { get; set; }
            public Expression<Func<WellnessProgramAttendance, WellnessProgramAttendance>> Select { get; set; }

            public List<(Expression<Func<WellnessProgramAttendance, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetWellnessProgramAttendanceQuery : IRequest<(List<WellnessProgramAttendanceDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<WellnessProgramAttendance, object>>> Includes { get; set; }
            public Expression<Func<WellnessProgramAttendance, bool>> Predicate { get; set; }
            public Expression<Func<WellnessProgramAttendance, WellnessProgramAttendance>> Select { get; set; }

            public List<(Expression<Func<WellnessProgramAttendance, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateWellnessProgramAttendance(Expression<Func<WellnessProgramAttendance, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<WellnessProgramAttendance, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateWellnessProgramAttendanceRequest(WellnessProgramAttendanceDto WellnessProgramAttendanceDto) : IRequest<WellnessProgramAttendanceDto>
        {
            public WellnessProgramAttendanceDto WellnessProgramAttendanceDto { get; set; } = WellnessProgramAttendanceDto;
        }

        public class BulkValidateWellnessProgramAttendance(List<WellnessProgramAttendanceDto> WellnessProgramAttendancesToValidate) : IRequest<List<WellnessProgramAttendanceDto>>
        {
            public List<WellnessProgramAttendanceDto> WellnessProgramAttendancesToValidate { get; } = WellnessProgramAttendancesToValidate;
        }

        public class CreateListWellnessProgramAttendanceRequest(List<WellnessProgramAttendanceDto> WellnessProgramAttendanceDtos) : IRequest<List<WellnessProgramAttendanceDto>>
        {
            public List<WellnessProgramAttendanceDto> WellnessProgramAttendanceDtos { get; set; } = WellnessProgramAttendanceDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateWellnessProgramAttendanceRequest(WellnessProgramAttendanceDto WellnessProgramAttendanceDto) : IRequest<WellnessProgramAttendanceDto>
        {
            public WellnessProgramAttendanceDto WellnessProgramAttendanceDto { get; set; } = WellnessProgramAttendanceDto;
        }

        public class UpdateListWellnessProgramAttendanceRequest(List<WellnessProgramAttendanceDto> WellnessProgramAttendanceDtos) : IRequest<List<WellnessProgramAttendanceDto>>
        {
            public List<WellnessProgramAttendanceDto> WellnessProgramAttendanceDtos { get; set; } = WellnessProgramAttendanceDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteWellnessProgramAttendanceRequest : IRequest<bool>
        {
            public long Id { get; set; }
            public List<long> Ids { get; set; }
        }

        #endregion DELETE
    }
}
