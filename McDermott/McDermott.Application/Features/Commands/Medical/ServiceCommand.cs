namespace McDermott.Application.Features.Commands.Medical
{
    public class ServiceCommand
    {
        #region GET

        public class GetSingleServiceQuery : IRequest<ServiceDto>
        {
            public List<Expression<Func<Service, object>>> Includes { get; set; }
            public Expression<Func<Service, bool>> Predicate { get; set; }
            public Expression<Func<Service, Service>> Select { get; set; }

            public List<(Expression<Func<Service, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetServiceQuery : IRequest<(List<ServiceDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Service, object>>> Includes { get; set; }
            public Expression<Func<Service, bool>> Predicate { get; set; }
            public Expression<Func<Service, Service>>? Select { get; set; }

            public List<(Expression<Func<Service, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateServiceQuery(List<ServiceDto> ServicesToValidate) : IRequest<List<ServiceDto>>
        {
            public List<ServiceDto> ServicesToValidate { get; } = ServicesToValidate;
        }

        public class ValidateServiceQuery(Expression<Func<Service, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Service, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateServiceRequest(ServiceDto ServiceDto) : IRequest<ServiceDto>
        {
            public ServiceDto ServiceDto { get; set; } = ServiceDto;
        }

        public class CreateListServiceRequest(List<ServiceDto> ServiceDtos) : IRequest<List<ServiceDto>>
        {
            public List<ServiceDto> ServiceDtos { get; set; } = ServiceDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateServiceRequest(ServiceDto ServiceDto) : IRequest<ServiceDto>
        {
            public ServiceDto ServiceDto { get; set; } = ServiceDto;
        }

        public class UpdateListServiceRequest(List<ServiceDto> ServiceDtos) : IRequest<List<ServiceDto>>
        {
            public List<ServiceDto> ServiceDtos { get; set; } = ServiceDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteServiceRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}