namespace McDermott.Application.Features.Commands.Transaction
{
    public class GeneralConsultanServiceCommand
    {
        #region GET

        // Deprecated
        public class GetGeneralConsultanServiceQuery(Expression<Func<GeneralConsultanService, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<GeneralConsultanServiceDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<GeneralConsultanService, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
        }

        public class GetGeneralConsultanServicesQuery : IRequest<(List<GeneralConsultanServiceDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<GeneralConsultanService, object>>> Includes { get; set; }
            public Expression<Func<GeneralConsultanService, bool>> Predicate { get; set; }
            public Expression<Func<GeneralConsultanService, GeneralConsultanService>> Select { get; set; }

            public List<(Expression<Func<GeneralConsultanService, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetSingleGeneralConsultanServicesQuery : IRequest<GeneralConsultanServiceDto>
        {
            public List<Expression<Func<GeneralConsultanService, object>>> Includes { get; set; }
            public Expression<Func<GeneralConsultanService, bool>> Predicate { get; set; }
            public Expression<Func<GeneralConsultanService, GeneralConsultanService>> Select { get; set; }

            public List<(Expression<Func<GeneralConsultanService, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateGeneralConsultanServiceQuery(Expression<Func<GeneralConsultanService, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<GeneralConsultanService, bool>> Predicate { get; } = predicate!;
        }

        public class GetGeneralConsultanServiceCountQuery(Expression<Func<GeneralConsultanService, bool>>? predicate = null) : IRequest<int>
        {
            public Expression<Func<GeneralConsultanService, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region Create

        public class CreateGeneralConsultanServiceRequest(GeneralConsultanServiceDto GeneralConsultanServiceDto) : IRequest<GeneralConsultanServiceDto>
        {
            public GeneralConsultanServiceDto GeneralConsultanServiceDto { get; set; } = GeneralConsultanServiceDto;
        }

        public class CreateListGeneralConsultanServiceRequest(List<GeneralConsultanServiceDto> GeneralConsultanServiceDtos) : IRequest<List<GeneralConsultanServiceDto>>
        {
            public List<GeneralConsultanServiceDto> GeneralConsultanServiceDtos { get; set; } = GeneralConsultanServiceDtos;
        }

        #endregion Create

        #region Update

        public class UpdateGeneralConsultanServiceRequest(GeneralConsultanServiceDto GeneralConsultanServiceDto) : IRequest<GeneralConsultanServiceDto>
        {
            public GeneralConsultanServiceDto GeneralConsultanServiceDto { get; set; } = GeneralConsultanServiceDto;
        }

        public class CancelGeneralConsultanServiceRequest(GeneralConsultanServiceDto GeneralConsultanServiceDto) : IRequest<GeneralConsultanServiceDto>
        {
            public GeneralConsultanServiceDto GeneralConsultanServiceDto { get; set; } = GeneralConsultanServiceDto;
        }

        public class CreateFormGeneralConsultanServiceNewRequest : IRequest<GeneralConsultanServiceDto>
        {
            public GeneralConsultanServiceDto? GeneralConsultanServiceDto { get; set; }
            public UserDto? UserDto { get; set; }
            public bool IsFollowUpPatient { get; set; } = false;
            public EnumStatusGeneralConsultantService Status { get; set; }
        }

        public class UpdateFormGeneralConsultanServiceNewRequest : IRequest<GeneralConsultanServiceDto>
        {
            public GeneralConsultanServiceDto? GeneralConsultanServiceDto { get; set; }
            public UserDto? UserDto { get; set; }
            public bool IsReferTo { get; set; } = false;
            public EnumStatusGeneralConsultantService Status { get; set; }
        }

        public class UpdateConfirmFormGeneralConsultanServiceNewRequest : IRequest<GeneralConsultanServiceDto>
        {
            public GeneralConsultanServiceDto? GeneralConsultanServiceDto { get; set; }
            public UserDto? UserDto { get; set; }
            public EnumStatusGeneralConsultantService Status { get; set; }
        }

        public class UpdateStatusGeneralConsultanServiceRequest : IRequest<GeneralConsultanServiceDto>
        {
            public long Id { get; set; }
            public EnumStatusGeneralConsultantService Status { get; set; }
        }

        public class UpdateListGeneralConsultanServiceRequest(List<GeneralConsultanServiceDto> GeneralConsultanServiceDtos) : IRequest<List<GeneralConsultanServiceDto>>
        {
            public List<GeneralConsultanServiceDto> GeneralConsultanServiceDtos { get; set; } = GeneralConsultanServiceDtos;
        }

        #endregion Update

        #region Delete

        public class DeleteGeneralConsultanServiceRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion Delete

        #region GET GeneralConsultan Logs

        public class GetGeneralConsultanServicesLogQuery : IRequest<(List<GeneralConsultationServiceLogDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<GeneralConsultationServiceLog, object>>> Includes { get; set; }
            public Expression<Func<GeneralConsultationServiceLog, bool>> Predicate { get; set; }
            public Expression<Func<GeneralConsultationServiceLog, GeneralConsultationServiceLog>> Select { get; set; }

            public List<(Expression<Func<GeneralConsultationServiceLog, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetSingleGeneralConsultanServicesLogQuery : IRequest<GeneralConsultationServiceLogDto>
        {
            public List<Expression<Func<GeneralConsultationServiceLog, object>>> Includes { get; set; }
            public Expression<Func<GeneralConsultationServiceLog, bool>> Predicate { get; set; }
            public Expression<Func<GeneralConsultationServiceLog, GeneralConsultationServiceLog>> Select { get; set; }

            public List<(Expression<Func<GeneralConsultationServiceLog, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        #endregion GET GeneralConsultan Logs

        #region Create GeneralConsultan Logs

        public class CreateGeneralConsultationLogRequest(GeneralConsultationServiceLogDto GeneralConsultanlogDto) : IRequest<GeneralConsultationServiceLogDto>
        {
            public GeneralConsultationServiceLogDto GeneralConsultanlogDto { get; set; } = GeneralConsultanlogDto;
        }

        public class CreateListGeneralConsultationLogRequest(List<GeneralConsultationServiceLogDto> GeneralConsultanlogDto) : IRequest<List<GeneralConsultationServiceLogDto>>
        {
            public List<GeneralConsultationServiceLogDto> GeneralConsultanlogDto { get; set; } = GeneralConsultanlogDto;
        }

        #endregion Create GeneralConsultan Logs

        #region Update GeneralConsultan Logs

        public class UpdateGeneralConsultationLogRequest(GeneralConsultationServiceLogDto GeneralConsultanlogDto) : IRequest<GeneralConsultationServiceLogDto>
        {
            public GeneralConsultationServiceLogDto GeneralConsultanlogDto { get; set; } = GeneralConsultanlogDto;
        }

        public class UpdateListGeneralConsultationLogRequest(List<GeneralConsultationServiceLogDto> GeneralConsultanlogDto) : IRequest<List<GeneralConsultationServiceLogDto>>
        {
            public List<GeneralConsultationServiceLogDto> GeneralConsultanlogDto { get; set; } = GeneralConsultanlogDto;
        }

        #endregion Update GeneralConsultan Logs

        #region Delete GeneralConsultan Logs

        public class DeleteGeneralConsultationLogRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion Delete GeneralConsultan Logs
    }
}