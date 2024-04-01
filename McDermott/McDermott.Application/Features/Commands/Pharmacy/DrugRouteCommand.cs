

namespace McDermott.Application.Features.Commands.Pharmacy
{
    public class DrugRouteCommand
    {
        #region GET  

        public class GetDrugRouteQuery(Expression<Func<DrugRoute, bool>>? predicate = null, bool removeCache = false) : IRequest<List<DrugRouteDto>>
        {
            public Expression<Func<DrugRoute, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

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
