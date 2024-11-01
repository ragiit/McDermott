using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Transaction
{
    public class WellnessProgramCommand
    {
        #region GET

        public class GetSingleWellnessProgramQuery : IRequest<WellnessProgramDto>
        {
            public List<Expression<Func<WellnessProgram, object>>> Includes { get; set; }
            public Expression<Func<WellnessProgram, bool>> Predicate { get; set; }
            public Expression<Func<WellnessProgram, WellnessProgram>> Select { get; set; }

            public List<(Expression<Func<WellnessProgram, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetWellnessProgramQuery : IRequest<(List<WellnessProgramDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<WellnessProgram, object>>> Includes { get; set; }
            public Expression<Func<WellnessProgram, bool>> Predicate { get; set; }
            public Expression<Func<WellnessProgram, WellnessProgram>> Select { get; set; }

            public List<(Expression<Func<WellnessProgram, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateWellnessProgram(Expression<Func<WellnessProgram, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<WellnessProgram, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateWellnessProgramRequest(WellnessProgramDto WellnessProgramDto) : IRequest<WellnessProgramDto>
        {
            public WellnessProgramDto WellnessProgramDto { get; set; } = WellnessProgramDto;
        }

        public class BulkValidateWellnessProgram(List<WellnessProgramDto> WellnessProgramsToValidate) : IRequest<List<WellnessProgramDto>>
        {
            public List<WellnessProgramDto> WellnessProgramsToValidate { get; } = WellnessProgramsToValidate;
        }

        public class CreateListWellnessProgramRequest(List<WellnessProgramDto> WellnessProgramDtos) : IRequest<List<WellnessProgramDto>>
        {
            public List<WellnessProgramDto> WellnessProgramDtos { get; set; } = WellnessProgramDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateWellnessProgramRequest(WellnessProgramDto WellnessProgramDto) : IRequest<WellnessProgramDto>
        {
            public WellnessProgramDto WellnessProgramDto { get; set; } = WellnessProgramDto;
        }

        public class UpdateListWellnessProgramRequest(List<WellnessProgramDto> WellnessProgramDtos) : IRequest<List<WellnessProgramDto>>
        {
            public List<WellnessProgramDto> WellnessProgramDtos { get; set; } = WellnessProgramDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteWellnessProgramRequest : IRequest<bool>
        {
            public long Id { get; set; }
            public List<long> Ids { get; set; }
        }

        #endregion DELETE
    }
}
