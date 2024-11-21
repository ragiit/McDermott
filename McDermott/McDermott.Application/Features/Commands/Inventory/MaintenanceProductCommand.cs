namespace McDermott.Application.Features.Commands.Inventory
{
    public class MaintenanceProductCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetAllMaintenanceProductQuery(Expression<Func<MaintenanceProduct, bool>>? predicate = null, bool removeCache = false) : IRequest<List<MaintenanceProductDto>>
        {
            public Expression<Func<MaintenanceProduct, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetSingleMaintenanceProductQuery : IRequest<MaintenanceProductDto>
        {
            public List<Expression<Func<MaintenanceProduct, object>>> Includes { get; set; }
            public Expression<Func<MaintenanceProduct, bool>> Predicate { get; set; }
            public Expression<Func<MaintenanceProduct, MaintenanceProduct>> Select { get; set; }

            public List<(Expression<Func<MaintenanceProduct, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetMaintenanceProductQuery : IRequest<(List<MaintenanceProductDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<MaintenanceProduct, object>>> Includes { get; set; }
            public Expression<Func<MaintenanceProduct, bool>> Predicate { get; set; }
            public Expression<Func<MaintenanceProduct, MaintenanceProduct>> Select { get; set; }

            public List<(Expression<Func<MaintenanceProduct, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateMaintenanceProductQuery(Expression<Func<MaintenanceProduct, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<MaintenanceProduct, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateMaintenanceProductRequest(MaintenanceProductDto MaintenanceProductDto) : IRequest<MaintenanceProductDto>
        {
            public MaintenanceProductDto MaintenanceProductDto { get; set; } = MaintenanceProductDto;
        }

        public class CreateListMaintenanceProductRequest(List<MaintenanceProductDto> GeneralConsultanCPPTDtos) : IRequest<List<MaintenanceProductDto>>
        {
            public List<MaintenanceProductDto> MaintenanceProductDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateMaintenanceProductRequest(MaintenanceProductDto MaintenanceProductDto) : IRequest<MaintenanceProductDto>
        {
            public MaintenanceProductDto MaintenanceProductDto { get; set; } = MaintenanceProductDto;
        }

        public class UpdateListMaintenanceProductRequest(List<MaintenanceProductDto> MaintenanceProductDtos) : IRequest<List<MaintenanceProductDto>>
        {
            public List<MaintenanceProductDto> MaintenanceProductDtos { get; set; } = MaintenanceProductDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteMaintenanceProductRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}