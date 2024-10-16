using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Medical
{
    public class DoctorScheduleDetailCommand
    {
        #region GET

        public class GetDoctorScheduleDetailQuery : IRequest<(List<DoctorScheduleDetailDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<DoctorScheduleDetail, object>>> Includes { get; set; }
            public Expression<Func<DoctorScheduleDetail, bool>> Predicate { get; set; }
            public Expression<Func<DoctorScheduleDetail, DoctorScheduleDetail>> Select { get; set; }

            public List<(Expression<Func<DoctorScheduleDetail, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetSingleDoctorScheduleDetailQuery : IRequest<DoctorScheduleDetailDto>
        {
            public List<Expression<Func<DoctorScheduleDetail, object>>> Includes { get; set; }
            public Expression<Func<DoctorScheduleDetail, bool>> Predicate { get; set; }
            public Expression<Func<DoctorScheduleDetail, DoctorScheduleDetail>> Select { get; set; }

            public List<(Expression<Func<DoctorScheduleDetail, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateDoctorScheduleDetailQuery(Expression<Func<DoctorScheduleDetail, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<DoctorScheduleDetail, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateDoctorScheduleDetailRequest(DoctorScheduleDetailDto DoctorScheduleDetailDto) : IRequest<DoctorScheduleDetailDto>
        {
            public DoctorScheduleDetailDto DoctorScheduleDetailDto { get; set; } = DoctorScheduleDetailDto;
        }

        public class BulkValidateDoctorScheduleDetailQuery(List<DoctorScheduleDetailDto> DoctorScheduleDetailsToValidate) : IRequest<List<DoctorScheduleDetailDto>>
        {
            public List<DoctorScheduleDetailDto> DoctorScheduleDetailsToValidate { get; } = DoctorScheduleDetailsToValidate;
        }

        public class CreateListDoctorScheduleDetailRequest(List<DoctorScheduleDetailDto> DoctorScheduleDetailDtos) : IRequest<List<DoctorScheduleDetailDto>>
        {
            public List<DoctorScheduleDetailDto> DoctorScheduleDetailDtos { get; set; } = DoctorScheduleDetailDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateDoctorScheduleDetailRequest(DoctorScheduleDetailDto DoctorScheduleDetailDto) : IRequest<DoctorScheduleDetailDto>
        {
            public DoctorScheduleDetailDto DoctorScheduleDetailDto { get; set; } = DoctorScheduleDetailDto;
        }

        public class UpdateListDoctorScheduleDetailRequest(List<DoctorScheduleDetailDto> DoctorScheduleDetailDtos) : IRequest<List<DoctorScheduleDetailDto>>
        {
            public List<DoctorScheduleDetailDto> DoctorScheduleDetailDtos { get; set; } = DoctorScheduleDetailDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteDoctorScheduleDetailRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}