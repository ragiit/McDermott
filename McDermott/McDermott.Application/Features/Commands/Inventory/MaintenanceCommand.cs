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

        public class GetSingleMaintenanceQuery : IRequest<MaintenanceDto>
        {
            public List<Expression<Func<Maintenance, object>>> Includes { get; set; }
            public Expression<Func<Maintenance, bool>> Predicate { get; set; }
            public Expression<Func<Maintenance, Maintenance>> Select { get; set; }

            public List<(Expression<Func<Maintenance, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetMaintenanceQuery : IRequest<(List<MaintenanceDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Maintenance, object>>> Includes { get; set; }
            public Expression<Func<Maintenance, bool>> Predicate { get; set; }
            public Expression<Func<Maintenance, Maintenance>> Select { get; set; }

            public List<(Expression<Func<Maintenance, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
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