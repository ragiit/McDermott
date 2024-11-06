using McDermott.Application.Dtos.AwarenessEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.AwarenessEvent
{
    public class ParticipanEduCommand
    {
        #region GET Education Program Detail

        public class GetAllParticipanEduQuery(Expression<Func<ParticipanEdu, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ParticipanEduDto>>
        {
            public Expression<Func<ParticipanEdu, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        public class GetSingleParticipanEduQuery : IRequest<ParticipanEduDto>
        {
            public List<Expression<Func<ParticipanEdu, object>>> Includes { get; set; }
            public Expression<Func<ParticipanEdu, bool>> Predicate { get; set; }
            public Expression<Func<ParticipanEdu, ParticipanEdu>> Select { get; set; }

            public List<(Expression<Func<ParticipanEdu, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetParticipanEduQuery : IRequest<(List<ParticipanEduDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<ParticipanEdu, object>>> Includes { get; set; }
            public Expression<Func<ParticipanEdu, bool>> Predicate { get; set; }
            public Expression<Func<ParticipanEdu, ParticipanEdu>> Select { get; set; }

            public List<(Expression<Func<ParticipanEdu, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateParticipanEduQuery(List<ParticipanEduDto> ParticipanEduToValidate) : IRequest<List<ParticipanEduDto>>
        {
            public List<ParticipanEduDto> ParticipanEduToValidate { get; } = ParticipanEduToValidate;
        }

        public class ValidateParticipanEduQuery(Expression<Func<ParticipanEdu, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<ParticipanEdu, bool>> Predicate { get; } = predicate!;
        }
        #endregion GET Education Program Detail

        #region CREATE Education Program

        public class CreateParticipanEduRequest(ParticipanEduDto ParticipanEduDto) : IRequest<ParticipanEduDto>
        {
            public ParticipanEduDto ParticipanEduDto { get; set; } = ParticipanEduDto;
        }

        public class CreateListParticipanEduRequest(List<ParticipanEduDto> ParticipanEduDtos) : IRequest<List<ParticipanEduDto>>
        {
            public List<ParticipanEduDto> ParticipanEduDtos { get; set; } = ParticipanEduDtos;
        }

        #endregion CREATE Education Program

        #region UPDATE Education Program

        public class UpdateParticipanEduRequest(ParticipanEduDto ParticipanEduDto) : IRequest<ParticipanEduDto>
        {
            public ParticipanEduDto ParticipanEduDto { get; set; } = ParticipanEduDto;
        }

        public class UpdateListParticipanEduRequest(List<ParticipanEduDto> ParticipanEduDtos) : IRequest<List<ParticipanEduDto>>
        {
            public List<ParticipanEduDto> ParticipanEduDtos { get; set; } = ParticipanEduDtos;
        }

        #endregion Update Education Program

        #region DELETE Education Program

        public class DeleteParticipanEduRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Education Program

    }
}
