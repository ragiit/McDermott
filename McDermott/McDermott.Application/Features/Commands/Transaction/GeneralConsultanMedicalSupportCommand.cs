﻿namespace McDermott.Application.Features.Commands.Transaction
{
    public class GeneralConsultanMedicalSupportCommand
    {
        #region GET

        public class GetGeneralConsultanMedicalSupportQuery : IRequest<(List<GeneralConsultanMedicalSupportDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<GeneralConsultanMedicalSupport, object>>> Includes { get; set; }
            public Expression<Func<GeneralConsultanMedicalSupport, bool>> Predicate { get; set; }
            public Expression<Func<GeneralConsultanMedicalSupport, GeneralConsultanMedicalSupport>> Select { get; set; }

            public List<(Expression<Func<GeneralConsultanMedicalSupport, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetSingleGeneralConsultanMedicalSupportQuery : IRequest<GeneralConsultanMedicalSupportDto>
        {
            public List<Expression<Func<GeneralConsultanMedicalSupport, object>>> Includes { get; set; }
            public Expression<Func<GeneralConsultanMedicalSupport, bool>> Predicate { get; set; }
            public Expression<Func<GeneralConsultanMedicalSupport, GeneralConsultanMedicalSupport>> Select { get; set; }

            public List<(Expression<Func<GeneralConsultanMedicalSupport, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetSingleConfinedSpaceOrProcedureRoomQuery : IRequest<GeneralConsultanMedicalSupportDto>
        {
            public List<Expression<Func<GeneralConsultanMedicalSupport, object>>> Includes { get; set; }
            public Expression<Func<GeneralConsultanMedicalSupport, bool>> Predicate { get; set; }
            public Expression<Func<GeneralConsultanMedicalSupport, GeneralConsultanMedicalSupport>> Select { get; set; }

            public List<(Expression<Func<GeneralConsultanMedicalSupport, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateGeneralConsultanMedicalSupport(Expression<Func<GeneralConsultanMedicalSupport, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<GeneralConsultanMedicalSupport, bool>> Predicate { get; } = predicate!;
        }

        #region Get Log

        public class GetGeneralConsultanMedicalSupportLogQuery : IRequest<(List<GeneralConsultanMedicalSupportLogDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<GeneralConsultanMedicalSupportLog, object>>> Includes { get; set; }
            public Expression<Func<GeneralConsultanMedicalSupportLog, bool>> Predicate { get; set; }
            public Expression<Func<GeneralConsultanMedicalSupportLog, GeneralConsultanMedicalSupportLog>> Select { get; set; }

            public List<(Expression<Func<GeneralConsultanMedicalSupportLog, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetSingleGeneralConsultanMedicalSupportLogQuery : IRequest<GeneralConsultanMedicalSupportLogDto>
        {
            public List<Expression<Func<GeneralConsultanMedicalSupportLog, object>>> Includes { get; set; }
            public Expression<Func<GeneralConsultanMedicalSupportLog, bool>> Predicate { get; set; }
            public Expression<Func<GeneralConsultanMedicalSupportLog, GeneralConsultanMedicalSupportLog>> Select { get; set; }

            public List<(Expression<Func<GeneralConsultanMedicalSupportLog, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        #endregion Get Log

        #endregion GET

        #region CREATE

        public class CreateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupportDto) : IRequest<GeneralConsultanMedicalSupportDto>
        {
            public GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupportDto { get; set; } = GeneralConsultanMedicalSupportDto;
        }

        public class BulkValidateGeneralConsultanMedicalSupport(List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupportsToValidate) : IRequest<List<GeneralConsultanMedicalSupportDto>>
        {
            public List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupportsToValidate { get; } = GeneralConsultanMedicalSupportsToValidate;
        }

        public class CreateListGeneralConsultanMedicalSupportRequest(List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupportDtos) : IRequest<List<GeneralConsultanMedicalSupportDto>>
        {
            public List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupportDtos { get; set; } = GeneralConsultanMedicalSupportDtos;
        }

        #region Create Log

        public class CreateGeneralConsultanMedicalSupportLogRequest(GeneralConsultanMedicalSupportLogDto GeneralConsultanMedicalSupportLogDto) : IRequest<GeneralConsultanMedicalSupportLogDto>
        {
            public GeneralConsultanMedicalSupportLogDto GeneralConsultanMedicalSupportLogDto { get; set; } = GeneralConsultanMedicalSupportLogDto;
        }

        public class CreateListGeneralConsultanMedicalSupportLogRequest(List<GeneralConsultanMedicalSupportLogDto> GeneralConsultanMedicalSupportLogDtos) : IRequest<List<GeneralConsultanMedicalSupportLogDto>>
        {
            public List<GeneralConsultanMedicalSupportLogDto> GeneralConsultanMedicalSupportLogDtos { get; set; } = GeneralConsultanMedicalSupportLogDtos;
        }

        #endregion Create Log

        #endregion CREATE

        #region Update

        public class UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupportDto) : IRequest<GeneralConsultanMedicalSupportDto>
        {
            public GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupportDto { get; set; } = GeneralConsultanMedicalSupportDto;
        }

        public class UpdateListGeneralConsultanMedicalSupportRequest(List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupportDtos) : IRequest<List<GeneralConsultanMedicalSupportDto>>
        {
            public List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupportDtos { get; set; } = GeneralConsultanMedicalSupportDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteGeneralConsultanMedicalSupportRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}