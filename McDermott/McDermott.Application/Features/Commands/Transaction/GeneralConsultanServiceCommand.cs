namespace McDermott.Application.Features.Commands.Transaction
{
    public class GeneralConsultanServiceCommand
    {
        #region GET

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

        public class UpdateStatusGeneralConsultanServiceRequest(EnumStatusGeneralConsultantService status, long id) : IRequest<GeneralConsultanServiceDto>
        {
            public long Id { get; set; } = id;
            public EnumStatusGeneralConsultantService Status { get; set; } = status;
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

        public class GetGeneralConsultationLogQuery(Expression<Func<GeneralConsultationLog, bool>>? predicate = null, bool removeCache = false) : IRequest<List<GeneralConsultanlogDto>>
        {
            public Expression<Func<GeneralConsultationLog, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET GeneralConsultan Logs

        #region Create GeneralConsultan Logs

        public class CreateGeneralConsultationLogRequest(GeneralConsultanlogDto GeneralConsultanlogDto) : IRequest<GeneralConsultanlogDto>
        {
            public GeneralConsultanlogDto GeneralConsultanlogDto { get; set; } = GeneralConsultanlogDto;
        }

        public class CreateListGeneralConsultationLogRequest(List<GeneralConsultanlogDto> GeneralConsultanlogDto) : IRequest<List<GeneralConsultanlogDto>>
        {
            public List<GeneralConsultanlogDto> GeneralConsultanlogDto { get; set; } = GeneralConsultanlogDto;
        }

        #endregion Create GeneralConsultan Logs

        #region Update GeneralConsultan Logs

        public class UpdateGeneralConsultationLogRequest(GeneralConsultanlogDto GeneralConsultanlogDto) : IRequest<GeneralConsultanlogDto>
        {
            public GeneralConsultanlogDto GeneralConsultanlogDto { get; set; } = GeneralConsultanlogDto;
        }

        public class UpdateListGeneralConsultationLogRequest(List<GeneralConsultanlogDto> GeneralConsultanlogDto) : IRequest<List<GeneralConsultanlogDto>>
        {
            public List<GeneralConsultanlogDto> GeneralConsultanlogDto { get; set; } = GeneralConsultanlogDto;
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