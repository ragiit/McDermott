namespace McDermott.Application.Features.Commands.Inventory
{
    public class MaintenanceCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetAllMaintenanceQuery(Expression<Func<Maintenance, bool>>? predicate = null, bool removeCache = false) : IRequest<List<MaintenanceDto>>
        {
            public Expression<Func<Maintenance, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetMaintenanceQuery(Expression<Func<Maintenance, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<MaintenanceDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Maintenance, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
        }

        public class ValidateMaintenanceQuery(Expression<Func<Maintenance, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Maintenance, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateMaintenanceRequest(MaintenanceDto MaintenanceDto) : IRequest<MaintenanceDto>
        {
            public MaintenanceDto MaintenanceDto { get; set; } = MaintenanceDto;
        }

        public class CreateListMaintenanceRequest(List<MaintenanceDto> GeneralConsultanCPPTDtos) : IRequest<List<MaintenanceDto>>
        {
            public List<MaintenanceDto> MaintenanceDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateMaintenanceRequest(MaintenanceDto MaintenanceDto) : IRequest<MaintenanceDto>
        {
            public MaintenanceDto MaintenanceDto { get; set; } = MaintenanceDto;
        }

        public class UpdateListMaintenanceRequest(List<MaintenanceDto> MaintenanceDtos) : IRequest<List<MaintenanceDto>>
        {
            public List<MaintenanceDto> MaintenanceDtos { get; set; } = MaintenanceDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteMaintenanceRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}