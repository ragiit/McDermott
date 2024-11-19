using McDermott.Application.Dtos.Pharmacies;

namespace McDermott.Application.Features.Commands.Pharmacies
{
    public class DrugRouteCommand
    {
        #region GET

        public class GetAllDrugRouteQuery(Expression<Func<DrugRoute, bool>>? predicate = null, bool removeCache = false) : IRequest<List<DrugRouteDto>>
        {
            public Expression<Func<DrugRoute, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetDrugRouteQuery(Expression<Func<DrugRoute, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<DrugRoute, object>>>? includes = null, Expression<Func<DrugRoute, DrugRoute>>? select = null) : IRequest<(List<DrugRouteDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<DrugRoute, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<DrugRoute, object>>> Includes { get; } = includes!;
            public Expression<Func<DrugRoute, DrugRoute>>? Select { get; } = select!;
        }

        public class BulkValidateDrugRouteQuery(List<DrugRouteDto> DrugRoutesToValidate) : IRequest<List<DrugRouteDto>>
        {
            public List<DrugRouteDto> DrugRoutesToValidate { get; } = DrugRoutesToValidate;
        }

        public class ValidateDrugRouteQuery(Expression<Func<DrugRoute, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<DrugRoute, bool>> Predicate { get; } = predicate!;
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