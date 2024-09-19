namespace McDermott.Application.Features.Commands.Medical
{
    public class ServiceCommand
    {
        #region GET

        public class GetServiceQuery(Expression<Func<Service, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<ServiceDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Service, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
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