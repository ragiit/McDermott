using McDermott.Application.Dtos.AwarenessEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.AwarenessEvent
{
    public class EducationProgramCommand
    {
        #region GET Education Program Detail

        public class GetAllEducationProgramQuery(Expression<Func<EducationProgram, bool>>? predicate = null, bool removeCache = false) : IRequest<List<EducationProgramDto>>
        {
            public Expression<Func<EducationProgram, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        public class GetSingleEducationProgramQuery : IRequest<EducationProgramDto>
        {
            public List<Expression<Func<EducationProgram, object>>> Includes { get; set; }
            public Expression<Func<EducationProgram, bool>> Predicate { get; set; }
            public Expression<Func<EducationProgram, EducationProgram>> Select { get; set; }

            public List<(Expression<Func<EducationProgram, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetEducationProgramQuery : IRequest<(List<EducationProgramDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<EducationProgram, object>>> Includes { get; set; }
            public Expression<Func<EducationProgram, bool>> Predicate { get; set; }
            public Expression<Func<EducationProgram, EducationProgram>> Select { get; set; }

            public List<(Expression<Func<EducationProgram, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateEducationProgramQuery(List<EducationProgramDto> EducationProgramToValidate) : IRequest<List<EducationProgramDto>>
        {
            public List<EducationProgramDto> EducationProgramToValidate { get; } = EducationProgramToValidate;
        }

        public class ValidateEducationProgramQuery(Expression<Func<EducationProgram, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<EducationProgram, bool>> Predicate { get; } = predicate!;
        }
        #endregion GET Education Program Detail

        #region CREATE Education Program

        public class CreateEducationProgramRequest(EducationProgramDto EducationProgramDto) : IRequest<EducationProgramDto>
        {
            public EducationProgramDto EducationProgramDto { get; set; } = EducationProgramDto;
        }

        public class CreateListEducationProgramRequest(List<EducationProgramDto> EducationProgramDtos) : IRequest<List<EducationProgramDto>>
        {
            public List<EducationProgramDto> EducationProgramDtos { get; set; } = EducationProgramDtos;
        }

        #endregion CREATE Education Program

        #region UPDATE Education Program

        public class UpdateEducationProgramRequest(EducationProgramDto EducationProgramDto) : IRequest<EducationProgramDto>
        {
            public EducationProgramDto EducationProgramDto { get; set; } = EducationProgramDto;
        }

        public class UpdateListEducationProgramRequest(List<EducationProgramDto> EducationProgramDtos) : IRequest<List<EducationProgramDto>>
        {
            public List<EducationProgramDto> EducationProgramDtos { get; set; } = EducationProgramDtos;
        }

        #endregion Update Education Program

        #region DELETE Education Program

        public class DeleteEducationProgramRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Education Program

    }
}
