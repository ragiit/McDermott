namespace McDermott.Application.Features.Commands.Pharmacies
{
    public class DrugRouteCommand
    {
        #region GET

        public class GetSingleDrugRouteQuery : IRequest<DrugRouteDto>
        {
            public List<Expression<Func<DrugRoute, object>>> Includes { get; set; }
            public Expression<Func<DrugRoute, bool>> Predicate { get; set; }
            public Expression<Func<DrugRoute, DrugRoute>> Select { get; set; }

            public List<(Expression<Func<DrugRoute, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetDrugRouteQuery : IRequest<(List<DrugRouteDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<DrugRoute, object>>> Includes { get; set; }
            public Expression<Func<DrugRoute, bool>> Predicate { get; set; }
            public Expression<Func<DrugRoute, DrugRoute>> Select { get; set; }

            public List<(Expression<Func<DrugRoute, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateDrugRouteQuery(Expression<Func<DrugRoute, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<DrugRoute, bool>> Predicate { get; } = predicate!;
        }

        public class BulkValidateDrugRouteQuery(List<DrugRouteDto> DrugRoutesToValidate) : IRequest<List<DrugRouteDto>>
        {
            public List<DrugRouteDto> DrugRoutesToValidate { get; } = DrugRoutesToValidate;
        }

        #endregion GET

        #region CREATE

        public class CreateDrugRouteRequest(DrugRouteDto DrugRouteDto) : IRequest<DrugRouteDto>
        {
            public DrugRouteDto DrugRouteDto { get; set; } = DrugRouteDto;
        }

        public class CreateListDrugRouteRequest(List<DrugRouteDto> DrugRoutetos) : IRequest<List<DrugRouteDto>>
        {
            public List<DrugRouteDto> DrugRouteDtos { get; set; } = DrugRoutetos;
        }

        #endregion CREATE

        #region Update

        public class UpdateDrugRouteRequest(DrugRouteDto DrugRouteDto) : IRequest<DrugRouteDto>
        {
            public DrugRouteDto DrugRouteDto { get; set; } = DrugRouteDto;
        }

        public class UpdateListDrugRouteRequest(List<DrugRouteDto> DrugRouteDtos) : IRequest<List<DrugRouteDto>>
        {
            public List<DrugRouteDto> DrugRouteDtos { get; set; } = DrugRouteDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteDrugRouteRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}